// 1: 获取etc下面全部以conf结尾的文件
files, err := filepath.Glob("/etc/*.conf")

//2: 监控文件变动,可以使用以下库
//github.com/fsnotify/fsnotify
