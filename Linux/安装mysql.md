安装mysql时，要删除机子上的mariadb

``` sh
#linux启动mysql
service mysql start

#linux 停止mysql
service mysql stop

#说明:username - 你将创建的用户名, 
#host - 指定该用户在哪个主机上可以登陆,如果是本地用户可用localhost, 如果想让该用户可以从任意远程主机登陆,可以使用通配符%. 
#password - 该用户的登陆密码,密码可以为空,如果为空则该用户可以不需要密码登录服务器. 
#CREATE USER 'zpc'@'localhost' IDENTIFIED BY '123456'; 
#CREATE USER 'zpc'@'192.168.1.101_' IDENDIFIED BY '123456'; 
#CREATE USER 'zpc'@'%' IDENTIFIED BY '123456'; 
#CREATE USER 'zpc'@'%' IDENTIFIED BY ''; 
#CREATE USER 'zpc'@'%'; 
CREATE USER 'username'@'host' IDENTIFIED BY 'password'; 
```


地址：https://www.cnblogs.com/logaa/p/6791819.html 启动服务后，可以试mysql
