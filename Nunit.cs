/*此用于VS2013中*/

/*
1： 使用Nuget安装 Nunit
2:  在vs的 Extensions and Updates（扩展程序与更新）中安装 
    NUnit Test Adapter && 
    NUnit Test Adapter for VS2012 and VS2013
*/

[TestFixture]
    public class OrderTester
    {
        [SetUp]
        public void Init()
        {
           
        }
        [Test]
        public  void GetOrderList()
        {
            IPerson person = new Person();
            var list = new Order(person).ConsoleSomething("aaa");

            Assert.AreNotSame(0, list.Count);
        }
    }

//在方法上点右键一般会有（运行测试 或者 调试测试)

//http://www.tuicool.com/articles/vMjYBn   vs2013安装的一些详细说明
