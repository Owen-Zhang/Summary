##### linux(server)
``` sh

1: rpm –qa | grep rsync #查看linux上是否安装了此应用
2: yum install -y rsync #没安装的情况下安装
3: #配制rsync.conf文件
    ################################################################
    # /etc/rsyncd: configuration file for rsync daemon mode
    # See rsyncd.conf man page for more options.
    # configuration example:
    uid = root
    gid = root
    use chroot = no
    strict modes = false
    read only = no
    timeout = 6000
    charset=UTF-8
    max connections = 40
    hosts allow = 192.168.0.101 #允许访问的ip地址
    incoming chmod = Du=rwx,Dog=rx,Fu=rwx,Fgo=rx
    # hosts deny = 0.0.0.0/0
    log file = /tmp/rsync.log #日志文件地址

    # pid file = /var/run/rsyncd.pid
    # exclude = lost+found/
    # transfer logging = yes
    # timeout = 900
    # ignore nonreadable = yes
    # dont compress   = *.gz *.tgz *.zip *.z *.Z *.rpm *.deb *.bz2
    [home]
        path = /root/test # 同步文件到哪个地方
        comment = rsync
        auth users = root #使用的账号
        secrets file = /etc/rsync/rsyncd.secrets #密码文件: 用户名:密码
        read only = no
    #################################################################

3: 如果限制了ip同步, 还需要将客户端的ip和主机名加到服务端的hosts中
4: echo 'rsyncuser:password' > /etc/rsync/rsyncd.secrets #写密码文件
5: rsync -4 --daemon --config=/etc/rsync/rsyncd.conf #启动rsync服务
6: echo 'rsync -4 --daemon --config=/etc/rsync/rsyncd.conf' >> /etc/rc.local 

```

###### 客户端(windows)
```
1：http://sourceforge.net/projects/rsyncforwindows/ #下载地址
2：安装程序，设置环境变量
3: 运行脚本:
   rsync  -vzrtopg --progress --delete --password-file=./golang.secrets ./ root@192.168.0.109::home
   文件路径只能用相对路径

```