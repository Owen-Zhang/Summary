#### 升级


##### 1.查找主机上关于Docker的软件包
```
# rpm -qa | grep docker – – 列出包含docker字段的软件的信息
docker-ce-18.09.2-3.el7.x86_64
docker-ce-cli-18.09.2-3.el7.x86_64
```

##### 2.使用yum remove卸载软件
```
# yum remove docker-ce-18.09.2-3.el7.x86_64
# yum removedocker-ce-cli-18.09.2-3.el7.x86_64
```

##### 3.使用curl升级到最新版
```
# curl -fsSL https://get.docker.com/ | sh
```

##### 4.设置Docker开机自启
```
# systemctl enable docker
```

##### 5.重启Docker
```
# systemctl start docker
```

##### 6 查看版本号
```
docker info
```