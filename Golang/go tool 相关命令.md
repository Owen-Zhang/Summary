##### 1 交叉编译
```
GOOS=linux GOARCH=amd64 go build  //编译linux版本
GOOS=windows GOARCH=386 go build  //编译windows版本
```

##### 2 条件编译
``` go
 // +build linux darwin
 // +build !release
 package test 

 func testaaa(){
 }

//编译时就可以这样使用
go build -tags "linux" 
//这个就只会编译linux版本的, 多条件时非常有用
//如:打包不同的环境代码, 多数据库使用(go build -tags "mysql")，等
```

##### 3 预处理
``` go

//go:generate go version
func main() {

}
//使用: go generate -x(会输出go当前的版本号)
```

##### 4 go vet  检测出来不可复制的类型是否被复制过
``` go
go vet main.go

//不可复制的类型
//atomic.Value, 
//sync.Mutex, 
//sync.Cond, 
//sync.RWMutex, 
//sync.Map, 
//sync.Pool, 
//sync.WaitGroup

```

##### 5 go run -race main.go 查看代码中是否有临界区竞争性
``` go
go run -race main.go
```






