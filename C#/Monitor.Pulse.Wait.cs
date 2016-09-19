static readonly object _locker = new object();
        static bool _go;

        internal static void Main()
        {
            new Thread(Work).Start(); //新线程会被阻塞，因为_go == false
            Console.ReadLine(); //等待用户输入

            lock (_locker)
            {
                _go = true; //改变阻塞条件
                Monitor.Pulse(_locker); //通知等待的队列。
            }
        }

        static void Work()
        {
            lock (_locker)
            {
                while (!_go) //只要_go字段是false，就等待。
                    Monitor.Wait(_locker); //在等待的时候，锁已经被释放了。
            }

            Console.WriteLine("被唤醒了");
        }
        
        // url: http://www.cnblogs.com/LoveJenny/archive/2011/05/31/2060777.html
