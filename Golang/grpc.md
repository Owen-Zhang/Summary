### grpc使用相关

#### 1 安装环境(linux)
``` sh

#前提: 最好将gopath/bin文件夹加入到环境变量中

#1 Protocol Buffer Compiler(protoc 安装)
#文档: https://grpc.io/docs/protoc-installation/
#下载地址:https://github.com/protocolbuffers/protobuf/releases
#将下载下来的zip文件解压,里面有一个bin文件夹-> protoc，最好将protoc copy到go path下面的bin文件夹中,也可以不用copy,直接将解压后的bin文件夹路径加到path环境变量中(/etc/profile 或者 .bash_profile)，加入后 source /etc/profile 让生效
#使用protoc --version查看版本号，必须保证是3.0以上

#2 安装protoc-gen-go工具
#文档:https://grpc.io/docs/languages/go/quickstart/
#下载代码会将生成的protoc-gen-go工具放在$gopath/bin文件夹中
go get github.com/golang/protobuf/protoc-gen-go #这个一定要注意，要用github上的才是最新的

#两个工具都以安装完成

#3 生成grpc文件: xxx.pb.go
protoc --go_out=plugins=grpc:../pb/ *.proto #grpc:后面是要保存的路径

```

#### 2 protobuf 语法
``` proto

//协议版本
syntax = "proto3";

//生成后的go package包名, 这个会生成相应的文件夹
option go_package = "helloworld";

//*.proto包名
package helloworld;

//定义接口
service Person {
    //保存用户方法
    rpc Save(Request) returns(Response){}
}

//定义枚举
enum PhoneType {
    MOBILE = 0;
    HOME = 1;
    WORK = 2;
}

//定义参数实体
message PhoneInfo {
    //required 必传,proto3也不支持了
    string number = 1;

    //optional 可传可不传 default 表示默认值 proto3不支持默认值
    optional PhoneType type = 2;
}

message Request {
    string name = 1;
    int32 id = 2;
    optional string email = 3;

    //表示多个,相当于go slice
    repeated PhoneInfo phones = 4;
}

message Response {
    string message = 1;
}

```

#### 3 服务端调用
``` go
    //开启端口listen
    listener, err := net.Listen("tcp", ":10090")
    if err != nil {
        fmt.Println(err)
        return
    }

    //创建grpc实例,这里可以传入相关的interceptor(拦截器)
    server := grpc.NewServer()

    //将实例与接口关联,注册服务
    pb.RegisterPersonServer(server, &PersonService{})

    //启动服务,处理业务
    if err := server.Serve(listener); err != nil {
        fmt.Println(err)
    }
```

#### 4 客户端调用
``` go
    //创建与服务端的联接
    con, err := grpc.Dial("192.168.0.102:10090", grpc.WithInsecure(), grpc.WithBlock())
	if err != nil {
		fmt.Println(err)
		return  
	}
    defer con.Close()
    
    //创建client实例
    client := pb.NewPersonClient(con)
	for {
		r, err := client.Save(context.Background(), &pb.Request{
			Name:  "test",
			Id:    1,
			Email: "11@qq.com",
			Phones: []*pb.PhoneInfo{
				{Number: "123456789", Type: pb.PhoneType_WORK},
				{Number: "789456", Type: pb.PhoneType_MOBILE},
			},
		})
		if err != nil {
			log.Fatalf("could not greet: %v", err)
		}
		log.Printf("Greeting: %s", r.GetMessage())
		time.Sleep(10 * time.Second)
	}
```