##### 使用gops监控程序运行情况

``` 
1: 能做的事情 
    当前有哪些go语言进程，哪些使用gops的go进程
    进程的概要信息
    进程的调用栈
    进程的内存使用情况
    构建程序的Go版本
    运行时统计信息
    trace
    cpu profile
    memory profile

2: 使用方法
   2.1 生成gops命令行工具：go get -u github.com/google/gops(也可以理解成一个客户端程序)
   2.2 引用包,和现有代码集成,向外提供接口供client(gops)访问
   func gopshttp() {
	go func() {
		if err := agent.Listen(agent.Options{
			Addr:            "0.0.0.0:8091",
			ConfigDir:       "/root/gops",
			ShutdownCleanup: true,
		}); err != nil {
			panic(err)
		}
	    }()

        for i := 0; i < 100; i++ {
            go func() {
                for {

                }
            }()
        }

        common.Shutdown()
    }
       
2:  gops命令详解
    gops is a tool to list and diagnose Go processes.

    Usage:
    gops <cmd> <pid|addr> ...
    gops <pid> # displays process info
    gops help  # displays this help message

    Commands:
    stack      Prints the stack trace.
    gc         Runs the garbage collector and blocks until successful.
    setgc	     Sets the garbage collection target percentage.
    memstats   Prints the allocation and garbage collection stats.
    version    Prints the Go version used to build the program.
    stats      Prints runtime stats.
    trace      Runs the runtime tracer for 5 secs and launches "go tool trace".
    pprof-heap Reads the heap profile and launches "go tool pprof".
    pprof-cpu  Reads the CPU profile and launches "go tool pprof".

    All commands require the agent running on the Go process.
    "*" indicates the process is running the agent.

3：gops
   查看当前机器上的go进程，可以列出pid、ppid、进程名、可执行程序所使用的go版本，以及可执行程序的路径,带*的是程序中使用了gops/agent，不带*的是普通的go程序

4: gops 进程id(pid) #查看进程总体信息
    parent PID:	66333
    threads:	7
    memory usage:	0.018%
    cpu usage:	0.000%
    username:	shitaibin
    cmd+args:	./gops
    elapsed time:	11:28
    local/remote:	127.0.0.1:54753 <-> :0 (LISTEN)

5: gops stack 192.168.0.109:8091 #查看当前调用栈信息
    goroutine 19 [running]:
        runtime/pprof.writeGoroutineStacks(0x1197160, 0xc00009c028, 0x0, 0x0)
    /Users/shitaibin/goroot/src/runtime/pprof/pprof.go:679 +0x9d
        runtime/pprof.writeGoroutine(0x1197160, 0xc00009c028, 0x2, 0x0, 0x0)
    /Users/shitaibin/goroot/src/runtime/pprof/pprof.go:668 +0x44
        runtime/pprof.(*Profile).WriteTo(0x1275c60, 0x1197160, 0xc00009c028, 0x2, 0xc00009c028, 0x0)
    /Users/shitaibin/goroot/src/runtime/pprof/pprof.go:329 +0x3da

6: gops memstats 192.168.0.109:8091 #查看当前进程的内存信息
    alloc: 136.80KB (140088 bytes) // 当前分配出去未收回的内存总量
    total-alloc: 152.08KB (155728 bytes) // 已分配出去的内存总量
    sys: 67.25MB (70518784 bytes) // 当前进程从OS获取的内存总量
    lookups: 0
    mallocs: 418 // 分配的对象数量
    frees: 82 // 释放的对象数量
    heap-alloc: 136.80KB (140088 bytes) // 当前分配出去未收回的堆内存总量
    heap-sys: 63.56MB (66650112 bytes) // 当前堆从OS获取的内存
    heap-idle: 62.98MB (66035712 bytes) // 当前堆中空闲的内存量
    heap-in-use: 600.00KB (614400 bytes) // 当前堆使用中的内存量
    heap-released: 62.89MB (65945600 bytes)
    heap-objects: 336 // 堆中对象数量
    stack-in-use: 448.00KB (458752 bytes) // 栈使用中的内存量 
    stack-sys: 448.00KB (458752 bytes) // 栈从OS获取的内存总量 
    stack-mspan-inuse: 10.89KB (11152 bytes)
    stack-mspan-sys: 16.00KB (16384 bytes)
    stack-mcache-inuse: 13.56KB (13888 bytes)
    stack-mcache-sys: 16.00KB (16384 bytes)
    other-sys: 1.01MB (1062682 bytes)
    gc-sys: 2.21MB (2312192 bytes)
    next-gc: when heap-alloc >= 4.00MB (4194304 bytes) // 下次GC的条件
    last-gc: 2020-03-16 10:06:26.743193 +0800 CST // 上次GC的时间
    gc-pause-total: 83.84µs // GC总暂停时间
    gc-pause: 44891 // 上次GC暂停时间，单位纳秒
    num-gc: 2 // 已进行的GC次数
    enable-gc: true // 是否开始GC
    debug-gc: false

7: gops stats 进程id(pid)
    goroutines: 3
    OS threads: 12
    GOMAXPROCS: 8
    num CPU: 8

8: gops trace 192.168.0.109:8091 #获取当前运行5s的trace信息,会打开新的网页
    Tracing now, will take 5 secs...
    Trace dump saved to: /var/folders/5g/rz16gqtx3nsdfs7k8sb80jth0000gn/T/trace116447431
    2020/03/16 10:23:37 Parsing trace...
    2020/03/16 10:23:37 Splitting trace...
    2020/03/16 10:23:37 Opening browser. Trace viewer is listening on http://127.0.0.1:55480

    8.1 #打开网页就能看到以下信息
        View trace：查看跟踪 (按照时间分段，上面我的例子时间比较短，所以没有分段)
        Goroutine analysis：Goroutine 分析
        Network blocking profile：网络阻塞概况
        Synchronization blocking profile：同步阻塞概况
        Syscall blocking profile：系统调用阻塞概况
        Scheduler latency profile：调度延迟概况
        User defined tasks：用户自定义任务
        User defined regions：用户自定义区域
    8.1.1 View trace #点进去是图
          w 放大， s 缩小， a 左移， d 右移

9: gops pprof-cpu 192.168.0.109:8091 #查看cpu使用情况
   Profiling CPU now, will take 30 secs...
    Profile dump saved to: C:\Users\ADMINI~1\AppData\Local\Temp\cpu_profile488980503
    Binary file saved to: C:\Users\ADMINI~1\AppData\Local\Temp\binary882265738
    Type: cpu
    Time: Dec 7, 2020 at 2:01pm (CST)
    Duration: 32.20s, Total samples = 29.86s (92.74%)
    Entering interactive mode (type "help" for commands, "o" for options)

    9.1: web #会以网页打开的方式显示相关的信息
    9.2: top #显示主要函数的cpu使用情况
       Showing nodes accounting for 29.86s, 100% of 29.86s total
        flat  flat%   sum%        cum   cum%
        28.08s 94.04% 94.04%     29.70s 99.46%  main.gopshttp.func2
        1.62s  5.43% 99.46%      1.62s  5.43%  runtime.asyncPreempt
        0.16s  0.54%   100%      0.16s  0.54%  runtime.futex
         0     0%   100%      0.16s  0.54%  runtime.futexsleep
         0     0%   100%      0.16s  0.54%  runtime.mcall
         0     0%   100%      0.16s  0.54%  runtime.notesleep
         0     0%   100%      0.16s  0.54%  runtime.park_m
         0     0%   100%      0.16s  0.54%  runtime.schedule
         0     0%   100%      0.16s  0.54%  runtime.stoplockedm

10: gops pprof-heap 192.168.0.109:8091 #查看heap使用情况
    和cpu的使用方式相应,可以通过web 和 top查看,其它命令在研究中

11: gops version 192.168.0.109:8091  # 查看go的版本
12: gops gc 192.168.0.109:8091 #让程序运行一次gc
```
