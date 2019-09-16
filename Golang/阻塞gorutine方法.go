空的死循环会比较吃 CPU,因为它是一直运行且不会阻塞,而接收 chan 是阻塞的,会让出 CPU 执行时间片。简单验证下，分别按两种方式编写程序，top 看下就能明白了。

对于 Go 而言,空循环还有个缺点,如果程序中还存在其他 goroutine,但空循环因为没有阻塞，会一直占的 CPU，将可能导致其他 goroutine 得不到执行。

如果想死循环且不希望被阻塞，同时其他 goroutine 依然可以运行，可以试试下面的写法：

func main() {
    go func () {
        // do something
    }()
    for {
        runtime.Gosched()
    }
}

runtime.Gosched 用于让出 CPU 时间片给其他 goroutine，如果没有任务要执行，继续下次循环。
要实现阻塞效果，有很多方式可以实现，比如接收 nil 的 chan，重复加锁，空 select，WaitGroup Add 无 Done 的 Wait 等。

