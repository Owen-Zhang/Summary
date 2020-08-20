##### 安装

1: 下载redis包
2: 解压tar包
3: make 成功后 在src目录下面会生成可运行文件redis-server redis-cli
4: 修改redis.conf配制(最好copy一份),大概修改点
```
1 bind 地址要改,改成机器ip地址
2 daemonize 改成yes(守护进程启动方式)
3 pidfile 要修改
4 logfile地址修改
5 dir地址修改
  port 如果一台机器多个实例也要修改
6 cluster-enabled yes 打开
7 cluster-config-file 打开
8 cluster-replica-validity-factor 打开
9 cluster-migration-barrier 打开
```

5 启动: redis-server redis.conf 
如果配制了log地址，如有错误里面会有说明

6 查看启动情况: ps -ef | grep redis   或者 netstat -ntlp | grep 端口号
7 初始集群
redis-cli --cluster create 192.168.0.109:6379 192.168.0.110:6379 192.168.0.111:6379 192.168.0.110:6380 192.168.0.109:6380  192.168.0.111:6380 --cluster-replicas 1
前几个为master,后面几个为slaver

8 客户端登陆
./redis-cli -c -h 192.168.0.109 -p 6379
登陆到集群的某个节点 -c以集群的方式登陆(否则只能操作当前机器) -h -p 分别指定host和端口

9 查看集群信息
cluster nodes 查看节点
cluster info 查看集群信息

10 操作(set get... )

11 维护操作
redis-cli --help  这个是外层的主要命令
redis-cli --cluster help --这个集群管理的一些帮助命令
```
redis-cli --cluster create 增加节点
redis-cli --cluster check 检查集群情况
redis-cli --cluster info 查看master节点信息以及keys的分布情况
.....
```

###### 安装注意点
如果是一个机器安装多个实例,不要占用一个特殊的端口就是配制文件中(port) 前面加1的端口
这个端口可能用于redis实例之间的通信



###### 文件内容替换
将redis7000/redis.conf 中的7000替换成70001并保存到redis70001/redis.conf文件中
sed 's/7000/7001/g' redis7000/redis.conf > redis70001/redis.conf

