
centos hosts 
/etc/resolv.conf 
nameserver 192.168.106.189
nameserver 192.168.106.190

但是重启网络后会失效，因为/etc/resolv.conf文件被还原了

解决办法就是锁定这个文件
chattr +i /etc/resolv.conf

如果想修改此文件，可以解锁后修改
chattr -i /etc/resolv.conf
