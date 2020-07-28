##### pprof方式查找cpu、内存使用情况

原文地址:里面有很好的例子
https://blog.wolfogre.com/posts/go-ppof-practice/

###### 前提
```
在main中引入_ "net/http/pprof"
通过goroute 启动httpserver, gin中是否可以同用一个端口还需要验证
go func() {
    // 启动一个 http server，注意 pprof 相关的 handler 已经自动注册过了
    if err := http.ListenAndServe(":6060", nil); err != nil {
        log.Fatal(err)
    }
}()

之后 http://localhost:6060/debug/pprof 可以看到相关的信息,了解大概情况,细看有问题的选项
可以使用后面说到的go tool pprof 相关的命令来得到一些详情
```

###### 查看cpu、内存等
```
先使用 top 命令看看进程是cpu高还是内存高
```

###### 1 查看cpu使用情况
```
1: go tool pprof http://127.0.0.1:6060/debug/pprof/profile #默认获取30秒中的cpu信息
生成xx.gz文件到本地后
2: (pprof) top 可以看到cpu较高的调用,一般都会看到文件名(还有方法)
3: (pprof) list Eat 可以看代码那一行的使用时间，很容易定位到问题
4: (pprof) web 可以通过网页的方式展示出cpu使用情况(每个节点都有标记),打开了.svg文件
```
###### 2 排查内存占用过高
```
1: go tool pprof http://127.0.0.1:6060/debug/pprof/heap
2: (pprof) top 查看较高的调用
3: (pprof) list Eat 查看代码中占用内存的情况(可以定位到代码行)
4: (pprof) web 通过网页方式展示内存使用情况
```

###### 3 查看GC使用情况 (gc使用太频繁,程序一般都有优化空间)
对象是使用堆内存还是栈内存，由编译器进行逃逸分析并决定，如果对象不会逃逸，便可在使用栈内存，但总有意外，就是对象的尺寸过大时，便不得不使用堆内存,这时就需要gc去回收内存
```
1: GODEBUG=gctrace=1 go run main.go | grep gc 查看gc情况(时间、堆、大小)
2: go tool pprof http://localhost:6060/debug/pprof/allocs?seconds=60 #这个最好在程序运行一会再获取
3: (pprof) top 查看较高的调用
4: (pprof) list Eat 查看代码中回收内存的情况(可以定位到代码行)
5: (pprof) web 通过网页方式展示回收情况
```

###### 4 查看goroutine 查看协程使用情况
```
1: go tool pprof http://localhost:6060/debug/pprof/goroutine 
3: (pprof) top 查看较高的调用
4: (pprof) list Eat 查看代码中回收内存的情况(可以定位到代码行)
5: (pprof) web 通过网页方式展示回收情况
```


###### 5 排查锁的争用
go tool pprof http://localhost:6060/debug/pprof/mutex

###### 6 排查阻塞操作
go tool pprof http://localhost:6060/debug/pprof/block




