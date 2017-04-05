Console.WriteLine(System.Environment.MachineName);
Console.WriteLine(System.Net.Dns.GetHostName());
var result =  System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[1].ToString();
//如果找不到会抛出异常，要进行处理
var test = System.Net.Dns.GetHostEntry("sdfasd");
