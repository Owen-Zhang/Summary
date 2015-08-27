1： 判断一个类是否继承一个接口(非T)时： 
var impOther = Activator.CreateInstance(type) as IFilter;
if (impOther == null)
	Console.WriteLine("not implement class");
else
	impOther.FilteRequest();
如果判断是否继承一个类，直接 IsSubClass 就可以了
如果是T，就用 GetGenericTypeDefinition() != typeof(IService<>)

2: 使用Task来同时处理多个任务
var tt = new TestTasks();
var t1 = Task.Factory.StartNew(tt.Task1);
var t2 = Task.Factory.StartNew(() => tt.Task2(1000));
var t3 =Task.Factory.StartNew(() => tt.Task3(1000));

//等待t1,t2,t3执行完成
Task.WaitAll(t1,t2,t3);
Console.WriteLine(t3.Result);

3: ToString("#,##0.00"), 显示三位分隔如： 1235.23 => 1,235.23

4: redis window下cmd 运行：server: redis-server.exe d:\Redis\redis.windows.conf
						   client: redis-cli.exe -h 127.0.0.1 -p 6379
						   数据保存的文件在安装目录下，名为：dump.rdb
						   
5: Interlocked.Increment(ref 一个变量),  Interlocked.Decrement(ref 一个变量) 能保证线程安全(相关当于++, --)	
