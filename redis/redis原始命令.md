###### 1 原始命令
flushall 清除所有的key

##### string相关操作(包括bitmap)
help @string 可以查看string 相关的命令

1 字符串 
set k1 value 

2 向一个key追加字符
append k1 bbb  ### 结果为: "valuebbb"

3 计算一个字符串的字节长度
strlen k1  ##结果为 8, 字节长度,一个中文字一般会占用三个字节(utf-8)

4 加减
incr 加1 decr 减1，如果key是新增的，默认从0开始
incr k2 ## 结果为: 1
如果 incr k1 此时会报错, 如果能正常转成数字的话就会成功加1

5: 二进制相关
setbit k3  1 1    ### k3结果为: 01000000  '@'  前面的1 是index,从0开始,后面的值是1或0
setbit k3  7 1    ### k3结果为: 01000010  'A'

setbit k4  1 1    ### k4结果为: 01000000  '@' 
setbit k4  6 1    ### k4结果为: 01000100  'B'

bitop and reskey k3 k4  ### reskey的结果为: 01000000  '@' 
bitop or  orkey k3 k4   ### orkey的结果为: 01000110 'C'

使用场景: 记录用户一段时间的登陆次数 (还有很多使用场景)
setbit zhang 3 1 第四天登陆
setbit zhang 6 1 第7天登陆
setbit zhang 300 第301天登陆
bitcount zhang   ###结果为3,表示登陆了3天
strlen zhang     ### 整个占用46字节


##### list
help @list 查看list相关的命令

1: 增加
lpush k6 1 2 3 4 5 6 在左边插入相应的数组数据
lrange k6 0 -1 ### 结果为：6 5 4 3 2 1  查看k6的全部数据(以左边为起点) 范围(2,6 第三个数据到第7个)
rpush k6 xx yy zz 从右边加入数据
lrange k6 7 8 以左边开始取范围内的数据

lpop k6 弹出左边第一个值 ### 结果为1
rpop k6 弹出右边第一个值 ### 结果为zz
ltrim k6 1 -2 清除两者之外的数据

使用场景: 队列、栈、数组



##### hash  hashmap
hset zhang name test //向set中增加属性
hset zhang age  26
hgetall zhang  ##返回所有的属性信息
hkeys zhang    ##返回zhang下面所有的keys
hincrby zhang age 2 ##将zhang的年龄增加2岁 负数为减

HDEL key field [field ...] Delete one or more hash fields
HEXISTS key field Determine if a hash field exists
HGET key field Get the value of a hash field
HMGET key field [field ...] Get the values of all the given hash fields
hmget zhang name age
HMSET key field value [field value ...] Set multiple hash fields to multiple values
hmset zhang name "test" age 28

使用场景: 聚集数据，详情页(收藏、评论数)这些数据还要支持动态修改
          用户页面(好友数据、粉丝数)


##### set 无序，不重复

视频地址: https://www.bilibili.com/video/BV13z411b7mU?p=4