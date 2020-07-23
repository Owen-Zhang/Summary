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
最好使用表格测试法
``` go
func TestIsBlank(t *testing.T) {
	type args struct {
		source string
	}

	tests := []struct {
		name string
		arg  args
		want bool
	}{
		{
			name: "测试1", arg: args{source: "用户名"}, want: false,
		},
		{
			name: "测试2", arg: args{source: ""}, want: true,
		},
		{
			name: "测试3", arg: args{source: " "}, want: true,
		},
	}
	for _, test := range tests {
		t.Run(test.name, func(t *testing.T) {
			if got := IsBlank(test.arg.source); got != test.want {
				t.Errorf("IsBlank() = %v, want = %v", got, test.want)
			}
		})
	}
}
```

###### 2. 性能测试