1: for range 里的value是值拷贝，如果是值类型，改了值是不会影响到原来的
2: 使用race检测数据竞争, 使用这些命令生成， 会给出相关读写地方提示
2.1：go build -race hi.go  ./hi
2.2：go run -race hi.go