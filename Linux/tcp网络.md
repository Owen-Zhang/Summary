##### 1 查看tcp半链接最大个数
``` sh
cat /proc/sys/net/ipv4/tcp_max_syn_backlog
```

##### 2 tcp全链接(accept queue)
``` sh
cat /proc/sys/net/core/somaxconn #查看内核设置的最大链接数
ss -lnt 'sport = :3306'          #查看
```

##### 3 ss 显示处于活动状态的套接字信息,比netstat性能更好
``` sh
-h：显示帮助信息；
-V：显示指令版本信息；
-n：不解析服务名称，以数字方式显示；
-a：显示所有的套接字；
-l：显示处于监听状态的套接字；
-o：显示计时器信息；
-m：显示套接字的内存使用情况；
-p：显示使用套接字的进程信息；
-i：显示内部的TCP信息；
-4：只显示ipv4的套接字；
-6：只显示ipv6的套接字；
-t：只显示tcp套接字；
-u：只显示udp套接字；
-d：只显示DCCP套接字；
-w：仅显示RAW套接字；
-x：仅显示UNIX域套接字。
```

``` sh
1: ss -pnl | grep mysql #mysql的网络使用情况
u_str LISTEN 0 80 /var/lib/mysql/mysql.sock 35845 * 0 users:(("mysqld",pid=1744,fd=23))
tcp LISTEN 0 80 :::3306 :::* users:(("mysqld",pid=1744,fd=22))
协议、状态、recv-q、send-q、ip和端口、文件信息
```

``` sh
LISTEN状态
Recv-Q：代表建立的连接还有多少没有被accept，比如Nginx接受新连接变的很慢
Send-Q：代表listen backlog值

ESTAB状态
Recv-Q：内核中的数据还有多少(bytes)没有被应用程序读取，发生了一定程度的阻塞
Send-Q：代表内核中发送队列里还有多少(bytes)数据没有收到ack，对端的接收处理能力不强

################################################################################
当 client 通过 connect 向 server 发出 SYN 包时，client 会维护一个 socket 等待队列，而 server 会维护一个 SYN 队列
此时进入半链接的状态，如果 socket 等待队列满了，server 则会丢弃，而 client 也会由此返回 connection time out；只要是 client 没有收到 SYN+ACK，3s 之后，client 会再次发送，如果依然没有收到，9s 之后会继续发送
半连接 syn 队列的长度为 max(64, /proc/sys/net/ipv4/tcp_max_syn_backlog)  决定
当 server 收到 client 的 SYN 包后，会返回 SYN, ACK 的包加以确认，client 的 TCP 协议栈会唤醒 socket 等待队列，发出 connect 调用
client 返回 ACK 的包后，server 会进入一个新的叫 accept 的队列，该队列的长度为 min(backlog, somaxconn)，默认情况下，somaxconn 的值为 128，表示最多有 129 的 ESTAB 的连接等待 accept()，而 backlog 的值则由 int listen(int sockfd, int backlog) 中的第二个参数指定，listen 里面的 backlog 的含义请看这里。需要注意的是，一些 Linux 的发型版本可能存在对 somaxcon 错误 truncating 方式。
当 accept 队列满了之后，即使 client 继续向 server 发送 ACK 的包，也会不被相应，此时，server 通过 /proc/sys/net/ipv4/tcp_abort_on_overflow 来决定如何返回，0 表示直接丢丢弃该 ACK，1 表示发送 RST 通知 client；相应的，client 则会分别返回 read timeout 或者 connection reset by peer。上面说的只是些理论，如果服务器不及时的调用 accept()，当 queue 满了之后，服务器并不会按照理论所述，不再对 SYN 进行应答，返回 ETIMEDOUT。根据这篇文档的描述，实际情况并非如此，服务器会随机的忽略收到的 SYN，建立起来的连接数可以无限的增加，只不过客户端会遇到延时以及超时的情况。

###########################################################
可以看到，整个 TCP stack 有如下的两个 queue:
1. 一个是 half open(syn queue) queue(max(tcp_max_syn_backlog, 64))，用来保存 SYN_SENT 以及 SYN_RECV 的信息。
2. 另外一个是 accept queue(min(somaxconn, backlog))，保存 ESTAB 的状态，但是调用 accept()。

LISTEN 状态: Recv-Q 表示的当前等待服务端调用 accept 完成三次握手的 listen backlog 数值，也就是说，当客户端通过 connect() 去连接正在 listen() 的服务端时，这些连接会一直处于这个 queue 里面直到被服务端 accept()；Send-Q 表示的则是最大的 listen backlog 数值，这就就是上面提到的 min(backlog, somaxconn) 的值。
其余状态: 非 LISTEN 状态之前理解的没有问题。Recv-Q 表示 receive queue 中的 bytes 数量；Send-Q 表示 send queue 中的 bytes 数值。
```

##### 4 iftop  查看某个ip的流量使用情况
``` sh
iftop -i 网卡名 -n -P # 查看ip流量使用情况 注意大小写
                     # -i设定监测的网卡
                     # -B 以bytes为单位显示流量(默认是bits)
                     # -n使host信息默认直接都显示IP
                     # -N使端口信息默认直接都显示端口号
                     # -P使host信息及端口信息默认就都显示;
```