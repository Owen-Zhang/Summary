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

9 查看集群信息(需要登陆某个客户端之后)
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
redis-cli --hotkeys 查看热点key
.....
```

``` shell
[root@master src]# ./redis-cli --cluster info 192.168.0.109:6379
192.168.0.109:6379 (7d9de2ab...) -> 0 keys | 5461 slots | 1 slaves.
192.168.0.111:6379 (735a9782...) -> 4 keys | 5461 slots | 1 slaves.
192.168.0.110:6379 (501f1e4f...) -> 0 keys | 5462 slots | 1 slaves.
[OK] 4 keys in 3 masters.
0.00 keys per slot on average.
```

``` shell
[root@master src]#./redis-cli --cluster check 192.168.0.109:6379
192.168.0.109:6379 (7d9de2ab...) -> 0 keys | 5461 slots | 1 slaves.
192.168.0.111:6379 (735a9782...) -> 4 keys | 5461 slots | 1 slaves.
192.168.0.110:6379 (501f1e4f...) -> 0 keys | 5462 slots | 1 slaves.
[OK] 4 keys in 3 masters.
0.00 keys per slot on average.
>>> Performing Cluster Check (using node 192.168.0.109:6379)
M: 7d9de2ab02f0da02698f1d5ecbd98ba3b23a5bd9 192.168.0.109:6379
   slots:[0-5460] (5461 slots) master
   1 additional replica(s)
M: 735a9782b3d9827534be5b6b101aceb182d9ce5c 192.168.0.111:6379
   slots:[10923-16383] (5461 slots) master
   1 additional replica(s)
S: c52405b6e076c308f8cb218df4a8d211328ceaf3 192.168.0.111:6380
   slots: (0 slots) slave
   replicates 501f1e4fa7c515f667ff91ed03baead8c3ec7302
S: a7961aea41698fb366d4d33573f4418b2e42ebfe 192.168.0.110:6380
   slots: (0 slots) slave
   replicates 7d9de2ab02f0da02698f1d5ecbd98ba3b23a5bd9
M: 501f1e4fa7c515f667ff91ed03baead8c3ec7302 192.168.0.110:6379
   slots:[5461-10922] (5462 slots) master
   1 additional replica(s)
S: 0c9eab4e3f033b008222436ae1e334981bf1bb69 192.168.0.109:6380
   slots: (0 slots) slave
   replicates 735a9782b3d9827534be5b6b101aceb182d9ce5c
[OK] All nodes agree about slots configuration.
>>> Check for open slots...
>>> Check slots coverage...
[OK] All 16384 slots covered.
```

###### 安装注意点
如果是一个机器安装多个实例,不要占用一个特殊的端口就是配制文件中(port) 前面加1的端口
这个端口可能用于redis实例之间的通信



###### 文件内容替换
将redis7000/redis.conf 中的7000替换成70001并保存到redis70001/redis.conf文件中
sed 's/7000/7001/g' redis7000/redis.conf > redis70001/redis.conf

