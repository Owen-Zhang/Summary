##### cpu 爆增原因查找方法

```
1: ssh 登陆查看os情况，是否是os问题等
2: lsof -i:端口号 查看进程端口监听情况 或者用 netstat -atunlp 查看进程端口占用情况
3: top 查看进程内存和cpu占用情况,可以找到是哪个进程的原因
4: pprof 这个至少要程序能正常反应才能得到相应的数据
5: 应用perf查看相关信息 
5.1: 安装perf  --yum install perf
6: perf top 用于实时显示当前系统的性能统计信息。该命令主要用来观察整个系统当前的状态，比如可以通过查看该命令的输出来查看当前系统最耗时的内核函数或某个用户进程。
7: perf stat ./testgo 下面有单独的说明
```

###### perf stat ./testgo (testgo是应用程序) 结束后会统计相关的信息
```
11,424.63 msec task-clock          #    0.872 CPUs utilized(cpu占用率)          
             1,490      context-switches  #    0.130 K/sec                  
                 0      cpu-migrations    #    0.000 K/sec                  
             1,367      page-faults       #    0.120 K/sec                  
   <not supported>      cycles                                                      
   <not supported>      instructions                                                
   <not supported>      branches                                                    
   <not supported>      branch-misses                                               

      13.097418605 seconds time elapsed

      11.338346000 seconds user
       0.078949000 seconds sys
```

```
Task-clock-msecs：CPU 利用率，该值高，说明程序的多数时间花费在 CPU 计算上而非 IO。
Context-switches：进程切换次数，记录了程序运行过程中发生了多少次进程切换，频繁的进程切换是应该避免的。
Cache-misses：程序运行过程中总体的 cache 利用情况，如果该值过高，说明程序的 cache 利用不好
CPU-migrations：表示进程 t1 运行过程中发生了多少次 CPU 迁移，即被调度器从一个 CPU 转移到另外一个 CPU 上运行。
Cycles：处理器时钟，一条机器指令可能需要多个 cycles，
Instructions: 机器指令数目。
IPC：是 Instructions/Cycles 的比值，该值越大越好，说明程序充分利用了处理器的特性。
Cache-references: cache 命中的次数
Cache-misses: cache 失效的次数。
```

##### 使用 perf record, 解读 report
```
使用 top 和 stat 之后，您可能已经大致有数了。要进一步分析，便需要一些粒度更细的信息。比如说您已经断定目标程序计算量较大，也许是因为有些代码写的不够精简。那么面对长长的代码文件，究竟哪几行代码需要进一步修改呢？这便需要使用 perf record 记录单个函数级别的统计信息，并使用 perf report 来显示统计结果。
```

```
perf record -e cpu-clock ./testgo (testgo可运行程序)
结束程序、查看报告
perf report 可以看到相关的信息
```