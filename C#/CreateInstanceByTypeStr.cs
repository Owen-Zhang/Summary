-----------------------------main-------------------------------------
namespace SP.Item
{
    internal class Program
    {
        /// <summary>
        /// args 第一个参数必须为sp.Item.类名，后面的就可以自已加
        /// 如: SP.Item.exe SP.Item.BatchMergeActionProcessTask sdfasdf 454555
        /// SP.Item.exe 为exe名, SP.Item.BatchMergeActionProcessTask为类名
        /// sdfasdf为参数名，454555也为参数名
        /// 在args参数中表示为： args[0] = “SP.Item.BatchMergeActionProcessTask”，
        /// args[0] = “sdfasdf”
        /// args[1] = “454555”
        /// </summary>
        private static void Main(string[] args)
        {
            try
            {
                //args = new string[] { "SP.Item.MergeMFRAndMPNMappingItemTask" };
                /* 发布的时候使用以下方式获取节点 */
                var taskName = args[0];
                if (string.IsNullOrWhiteSpace(taskName))
                    throw new Exception("please input TypeClass");

                JobManager.Run(Type.GetType(taskName), args);
            }
            catch (Exception err)
            {
                Console.Error.WriteLine(err.Message);
                Console.Error.WriteLine(err.StackTrace);
                if (err.InnerException != null)
                {
                    Console.Error.WriteLine("Inner Exception:");
                    Console.Error.WriteLine(err.InnerException.Message);
                    Console.Error.WriteLine(err.InnerException.StackTrace);
                }
                Environment.Exit(-1);
            }
        }
    }
}

----------------------------------------------------------------
namespace MKPL.Task.Framework
{
    public class JobManager
    {
        /// <summary>
        /// 执行TASK，支持自定义参数
        /// </summary>
        /// <param name="typeTask">Task 名称</param>
        /// <param name="extenParams">自定义参数，该参数需要在Task实现中自行处理，在Task的构造函数中接收该参数</param>
        public static void Run(Type typeTask, string[] args)
        {
            //Assembly assembly = Assembly.GetExecutingAssembly();
            //var typeTask = Type.GetType(taskName);
            if (typeTask == null || !typeof(BaseTask).IsAssignableFrom(typeTask)) return;

            var method = typeTask.GetMethod("Run");
            ConstructorInfo construct = typeTask.GetConstructor(new[] { typeof(string[]) });

            if (construct == null)
            {
                Console.WriteLine("{0} need constructor", typeTask.FullName);
                return;
            }
            object instance = construct.Invoke(new object [] {args});
            method.Invoke(instance, null);
        }
    }
}

-----------------------------------------------------------------------
public BatchMergeActionProcessTask(string[] args)
        {
        }

