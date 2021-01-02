### yum 相关操作

##### 1 相关命令
``` sh

#1 安装某个软件, -y 在安装过程中不会提示用户确认,直接安装
yum -y install xxxx 

#2 卸载某个软件 
yum remove xxx

#3 查看安装的列表中是否有mysql
yum list installed | grep mysql

#4 查看某个已安装的软件包信息, 这个可以查看软件的版本号
yum list mysql-community-client.x86_64

#5 查看某个软件包的依赖包
yum deplist mysql-community-client.x86_64

#6 显示软件包的描述信息和概要信息 
yum info mysql-community-client.x86_64

#7 升级,如果不加包名就相当于全部yum包都升级，加了包名只升级此包
yum update mysql-community-client.x86_64

#8 可视化图形界面 Yumex
yum install yumex
```