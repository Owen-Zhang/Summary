1: docker run 
   --name db 给容器取名字
   --env PASSWORD=1111 注入环境变量
   -d 放在后台运行
   镜像名称一般放在最后
   
   --link db:mysql 链接另一个容器(db)取别名为mysql
   -p 8080:80 80为容器端口,8080为此应用映射到宿主机的端口(此端口外网可以访问)
   --restart=always 始终自启动
   --net=none 设置容器不使用网络
   -v /var/sum:/var/sum 宿主机的目录与容器目录相到映射(volumn),前面的为宿主机目录
   -m, --memory=256m， 指定容器的内存上限
   
2: docker stats containerid 
   查看容器内存、cpu、网络使用情况
   如果不加containerid 就会显示所有容器的相关信息
   
3: docker exec -it containerid(容器别名也可以) /bin/bash
   进入某个容器,运行/bin/bash

4: docker build
   -f 指定要使用的Dockerfile路径 docker build -f /path/to/a/Dockerfile . 
   -t docker build -t runoob/ubuntu:v1 .  (runoob/ubuntu镜像名称、v1版本号)
   --no-cache 创建镜像的过程不使用缓存；
   --force-rm 设置镜像过程中删除中间容器

5: docker port containerid(容器别名也可以)
   此容器映射的端口地址信息

6:删除所有的容器
  docker rm -f $( docker ps -a -q )

7: docker network ls
   查看docker的网络