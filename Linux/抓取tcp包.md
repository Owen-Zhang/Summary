###### 1 使用tcpdump抓取tcp信息
```
tcpdump -nn -i ens33 port 80
抓取ens33网卡上,端口为80的流入流出数据
port 指定端口
-i 监视指定网络接口的数据包 如果不指定网卡，默认tcpdump只会监视第一个网络接口
-X(大写) 显示交互的内容
tcpdump host baidu ## host只抓取某个host的包数据(也可以用ip)
tcpdump -i eth0 src host hostname ## 抓取hostname发出的数据包(发出)
tcpdump -i eth0 dst host hostname ## 抓取发到hostname的数据包(接收)
tcpdump tcp port 23 and host 210.27.48.1 ## 可以用and 或者or关联多个条件, tcp/udp 指抓取哪种类型数据
```