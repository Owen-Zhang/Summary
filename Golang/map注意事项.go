1、去除无用、重复的存储使用
2、尽量少用map或map的value尽量少为指针，太多的指针类型会造成GC扫描时间的增加；
3、若是用map用作缓存存储，当每次只更新部分，更新的key若是偏差较大，会有可能造成内存逐渐增长而不释放的问题，可以通过定时拷贝map的方式来解决
4、释放map所占的内存 则通过map=nil
5、非线程安全
6、定义map后，可以读(返回零值),写数据报错
7、map初始化 make(map[int]string), map[int]string{}, 其中make后面的数字是key/value个数(相当于len(map))
8、map的key最好使用int,string


sync.Map的原理介绍：sync.Map里头有两个map一个是专门用于读的read map，另一个是才是提供读写的dirty map；优先读read map，
若不存在则加锁穿透读dirty map，同时记录一个未从read map读到的计数，当计数到达一定值，就将read map用dirty map进行覆盖。
