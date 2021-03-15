####  pprof, trace 相关深入研究

##### 1 Trace信息的收集和分析
###### 1 在代码中增加trace信息
``` go
trace.Start(os.Stderr) //将trace信息通过err通道输出
defer trace.Stop()

//也可以这样使用
f, _ := os.Create("trace.out")
trace.Start(f)
defer trace.Stop()

//xxx 代码
```


###### 2 编译生成相应的trace文件,并打开相应的文件
``` sh
#生成文件
go run main.go 2> trace.out

#通过网页打开相应的文件
go tool trace trace.out
```

###### 3 分析相应的内容
``` sh
#1 会打开网页
    # View trace：查看跟踪
    # Goroutine analysis：Goroutine 分析
    # Network blocking profile：网络阻塞概况
    # Synchronization blocking profile：同步阻塞概况
    # Syscall blocking profile：系统调用阻塞概况
    # Scheduler latency profile：调度延迟概况
    # User defined tasks：用户自定义任务
    # User defined regions：用户自定义区域
    # Minimum mutator utilization：最低 Mutator 利用率

#2 看的步骤
   #通过 shift + ? 查看所有的快捷键
   #2.1 首先看Scheduler latency profile看大概
   
   #2.2 Goroutine analysis
   #2.2.1 相关字段说明
   #2.2.2 Execution Time: 执行时间
   #2.2.3 Network Wait Time: 网络等待时间
   #2.2.4 Sync Block Time: 同步阻塞时间
   #2.2.5 Blocking Syscall Time： 调用阻塞时间(系统调用)
   #2.2.6 Scheduler Wait Time: 调度等待时间
   #2.2.7 GC Sweeping： GC 清扫
   #2.2.8 GC Pause： GC 暂停

   #2.3 View trace 查看跟踪(网页顶端:【Flow events】,可以选上)
   #2.3.1 时间线: 显示执行的时间单元
   #2.3.2 堆：显示执行期间的内存分配和释放情况
   #2.3.3 协程：显示在执行期间的每个 Goroutine 运行阶段有多少个协程在运行
   #2.3.4 OS 线程：显示在执行期间有多少个线程在运行 Syscall（InSyscall）、运行中（Running）
   #2.3.5 虚拟处理器(P) 每个虚拟处理器显示一行，虚拟处理器的数量一般默认为系统内核数
   #2.3.6 协程和事件：显示在每个虚拟处理器上有什么 Goroutine 正在运行，而连线行为代表事件关联
   #2.3.7 点击具体的 Goroutine 行为后可以看到其相关联的详细信息
    # Start：开始时间
    # Wall Duration：持续时间
    # Self Time：执行时间
    # Start Stack Trace：开始时的堆栈信息
    # End Stack Trace：结束时的堆栈信息
    # Incoming flow：输入流
    # Outgoing flow：输出流
    # Preceding events：之前的事件
    # Following events：之后的事件
    # All connected：所有连接的事件
  
  #2.4 
```

###### 4 其它
```sh
#工具使用快捷键 w:放大、s:缩小、d:向右、a:向左
```


#### pprof使用细节









地址: https://blog.csdn.net/u013474436/article/details/105232768/
