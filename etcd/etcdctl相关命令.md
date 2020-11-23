###### 命令使用说明

```
1：获取某个目录下的所有节点信息
./etcdctl --endpoints=http://192.168.0.109:2379 get --prefix /test/

2:向某个节点写入数据
./etcdctl --endpoints=http://192.168.0.109:2379 put  /test/111 {"aaa":"12122","other":12}

3:删除一个节点及子节点下面的所有信息
./etcdctl del --prefix /registry

4:查询etcd中的所有节点信息
./etcdctl get --prefix /

```