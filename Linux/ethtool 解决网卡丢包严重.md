##### 1 概述
 
```
最近业务上老有问题，查看发现overruns值不断增加，学习了一下相关的知识。发现数值也在不停的增加。发现这些 errors, dropped, overruns 表示的含义还不大一样。
```
 
```sh
[root@localhost ~]# ifconfig eth0
eth0: flags=4163<UP,BROADCAST,RUNNING,MULTICAST> mtu 1500
inet 192.168.1.135 netmask 255.255.255.0 broadcast 192.168.1.255
inet6 fe80::20c:29ff:fe9b:52d3 prefixlen 64 scopeid 0x20<link>
ether 00:0c:29:9b:52:d3 txqueuelen 1000 (Ethernet)
RX packets 833 bytes 61846 (60.3 KiB)
RX errors 0 dropped 0 overruns 0 frame 0
TX packets 122 bytes 9028 (8.8 KiB)
TX errors 0 dropped 0 overruns 0 carrier 0 collisions 0
```
 
```
(1) RX errors
表示总的收包的错误数量，这包括 too-long-frames 错误，Ring Buffer 溢出错误，crc 校验错误，帧同步错误，fifo overruns 以及 missed pkg 等等。
 

(2) RX dropped
表示数据包已经进入了 Ring Buffer，但是由于内存不够等系统原因，导致在拷贝到内存的过程中被丢弃。

 
(3) RX overruns
表示了 fifo 的 overruns，这是由于 Ring Buffer(aka Driver Queue) 传输的 IO 大于 kernel 能够处理的 IO 导致的，而 Ring Buffer 则是指在发起 IRQ 请求之前的那块 buffer。很明显，overruns 的增大意味着数据包没到 Ring Buffer 就被网卡物理层给丢弃了，而 CPU 无法即使的处理中断是造成 Ring Buffer 满的原因之一，上面那台有问题的机器就是因为 interruprs 分布的不均匀(都压在 core0)，没有做 affinity 而造成的丢包。

(4) RX frame
表示 misaligned 的 frames。
```
 

##### 2 网卡工作原理
###### 2.1 网卡收包
```
网线上的packet首先被网卡获取，网卡会检查packet的CRC校验，保证完整性，然后将packet头去掉，得到frame。网卡会检查MAC包内的目的MAC地址，如果和本网卡的MAC地址不一样则丢弃(混杂模式除外)。

网卡将frame拷贝到网卡内部的FIFO缓冲区，触发硬件中断。（如有ring buffer的网卡，好像frame可以先存在ring buffer里再触发软件中断（下篇文章将详细解释Linux中frame的走向），ring buffer是网卡和驱动程序共享，是设备里的内存，但是对操作系统是可见的，因为看到linux内核源码里网卡驱动程序是使用kcalloc来分配的空间，所以ring buffer一般都有上限，另外这个ring buffer size，表示的应该是能存储的frame的个数，而不是字节大小。另外有些系统的 ethtool 命令 并不能改变ring parameters来设置ring buffer的大小，暂时不知道为什么，可能是驱动不支持）

网卡驱动程序通过硬中断处理函数，构建sk_buff，把frame从网卡FIFO拷贝到内存skb中，接下来交给内核处理。（支持napi的网卡应该是直接放在ring buffer，不触发硬中断，直接使用软中断，拷贝ring buffer里的数据，直接输送给上层处理，每个网卡在一次软中断处理过程能处理weight个frame）过程中，网卡芯片对frame进行了MAC过滤，以减小系统负荷。（除了混杂模式）
```

###### 2.2 网卡发包
```
网卡驱动程序将IP包添加14字节的MAC头，构成frame（暂无CRC）。Frame（暂无CRC）中含有发送端和接收端的MAC地址，由于是驱动程序创建MAC头，所以可以随便输入地址，也可以进行主机伪装。
驱动程序将frame（暂无CRC）拷贝到网卡芯片内部的缓冲区，由网卡处理。
网卡芯片将未完全完成的frame（缺CRC）再次封装为可以发送的packet，也就是添加头部同步信息和CRC校验，然后丢到网线上，就完成一个IP报的发送了，所有接到网线上的网卡都可以看到该packet。
```

###### 2.3 网卡中断处理函数
```
产生中断的每个设备都有一个相应的中断处理程序，是设备驱动程序的一部分。每个网卡都有一个中断处理程序，用于通知网卡该中断已经被接收了，以及把网卡缓冲区的数据包拷贝到内存中。
当网卡接收来自网络的数据包时，需要通知内核数据包到了。网卡立即发出中断。内核通过执行网卡已注册的中断处理函数来做出应答。中断处理程序开始执行，通知硬件，拷贝最新的网络数据包到内存，然后读取网卡更多的数据包。
这些都是重要、紧迫而又与硬件相关的工作。内核通常需要快速的拷贝网络数据包到系统内存，因为网卡上接收网络数据包的缓存大小固定，而且相比系统内存也要小得多。所以上述拷贝动作一旦被延迟，必然造成网卡FIFO缓存溢出 - 进入的数据包占满了网卡的缓存，后续的包只能被丢弃，这也应该就是ifconfig里的overrun的来源。
当网络数据包被拷贝到系统内存后，中断的任务算是完成了，这时它把控制权交还给被系统中断前运行的程序。
```
 
###### 2.4 缓冲区访问
```
网卡的内核缓冲区，是在PC内存中，由内核控制，而网卡会有FIFO缓冲区，或者ring buffer，这应该将两者区分开。FIFO比较小，里面有数据便会尽量将数据存在内核缓冲中。
网卡中的缓冲区既不属于内核空间，也不属于用户空间。它属于硬件缓冲，允许网卡与操作系统之间有个缓冲；
内核缓冲区在内核空间，在内存中，用于内核程序，做为读自或写往硬件的数据缓冲区；
用户缓冲区在用户空间，在内存中，用于用户程序，做为读自或写往硬件的数据缓冲区；
另外，为了加快数据的交互，可以将内核缓冲区映射到用户空间，这样，内核程序和用户程序就可以同时访问这一区间了。
对于有ring buffer的网卡，ring buffer是由驱动与网卡共享的，所以内核可以直接访问ring buffer，一般拷贝frames的副本到自己的内核空间进行处理（deliver到上层协议，之后的一个个skb就是按skb的指针传递方式传递，直到用户获得数据，所以，对于ring buffer网卡，大量拷贝发生在frame从ring buffer传递到内核控制的计算机内存里）。
```
 

#### 3 丢包排查
```
网卡工作在数据链路层，数据量链路层，会做一些校验，封装成帧。我们可以查看校验是否出错，确定传输是否存在问题。然后从软件层面，是否因为缓冲区太小丢包。
```

###### 3.1 先查看硬件情况
```
一台机器经常收到丢包的报警，先看看最底层的有没有问题:

(1) 查看工作模式是否正常
[root@localhost ~]# ethtool eth0 | egrep 'Speed|Duplex'
Speed: 1000Mb/s
Duplex: Full

(2) 查看检验是否正常

[root@localhost ~]# ethtool eth0 | egrep 'Speed|Duplex'
Speed: 1000Mb/s
Duplex: Full
Speed，Duplex，CRC 之类的都没问题，基本可以排除物理层面的干扰。
```

###### 3.2 通过 ifconfig 可以看到 overruns 是否一直增大
``` sh
for i in `seq 1 100`; do ifconfig eth2 | grep RX | grep overruns; sleep 1; done
```
```
这里一直增加
RX packets:346547657 errors:0 dropped:0 overruns:35345 frame:0
```
 

###### 3.3 查看buffer大小
 ```
可以通过ethtool来修改网卡的buffer size ，首先要网卡支持，我的服务器是是INTEL 的1000M网卡,我们看看ethtool说明 
-g   –show-ringQueries the specified ethernet device for rx/tx ring parameter information.
-G   –set-ringChanges the rx/tx ring parameters of the specified ethernet device.

(1) 查看当前网卡的buffer size情况
ethtool -g eth0

[root@localhost ~]# ethtool -g eth0
Ring parameters for eth0:
Pre-set maximums:
RX: 4096
RX Mini: 0
RX Jumbo: 0
TX: 4096
Current hardware settings:
RX: 256
RX Mini: 0
RX Jumbo: 0
TX: 256
```

###### 3.4 修改buffer size大小
``` sh
ethtool -G eth0 rx 2048
ethtool -G eth0 tx 2048
 
[root@localhost ~]# ethtool -G eth0 rx 2048
[root@localhost ~]#
[root@localhost ~]#
[root@localhost ~]# ethtool -G eth0 tx 2048
[root@localhost ~]#
[root@localhost ~]#
[root@localhost ~]# ethtool -g eth0
Ring parameters for eth0:
Pre-set maximums:
RX: 4096
RX Mini: 0
RX Jumbo: 0
TX: 4096
Current hardware settings:
RX: 2048
RX Mini: 0
RX Jumbo: 0
TX: 2048
```