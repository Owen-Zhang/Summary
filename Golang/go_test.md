##### 单元测试

###### 1. 方法测试
```
go test -v -run ^TestQueue$ 
后面使用的是正则
-v 显示详细信息
-run 运行某个方法
要运行某个文件中的某个方法, 如果测试的方法调用了另一个文件，必须将另一个文件写到后面
如： go test -v string_test.go string.go #运行string_test.go中的单元测试方法
     go test -v -run TestIsBlank string_test.go string.go  #运行string_test.go中的TestIsBlank方法
```

###### 2. 性能测试