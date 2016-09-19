// 获取程序的基目录。
System.AppDomain.CurrentDomain.BaseDirectory

// 获取模块的完整路径，包含文件名
System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName

// 获取和设置当前目录(该进程从中启动的目录)的完全限定目录。
System.Environment.CurrentDirectory

// 获取应用程序的当前工作目录，注意工作目录是可以改变的，而不限定在程序所在目录。
System.IO.Directory.GetCurrentDirectory()

// 获取和设置包括该应用程序的目录的名称。
System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase

// 获取启动了应用程序的可执行文件的路径。
System.Windows.Forms.Application.StartupPath

// 获取启动了应用程序的可执行文件的路径及文件名
System.Windows.Forms.Application.ExecutablePath
