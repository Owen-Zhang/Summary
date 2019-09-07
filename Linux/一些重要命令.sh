1: ll 列出文件和目录信息
   -a 列出所有的文件
   -h 文件大小显示出 k, m 
   -i 文件数字标识
   -d 只显示指定目录的信息
   
2: mkdir 创建目录
   -p 递归创建目录，上级目录不存在就创建
   
3: cp 复制文件或者目录
   -r 复制目录
   -f 如果有相同就覆盖
   -p 保持文件的属性不变
   cp ./a.txt ./b.txt /root 将a.txt和b.txt复制到root目录下(复制多个文件)
   cp 过去还可以改名
   
4: mv 移动文件或者改名

5: cat 查看文件
   -n 可以看到行号
   
6: less 查看文件内容 less /etc/services
   可以向下看，向上看
   还可以查找 /a  查找a字符 按n(next)表示查看下一次查找的位置
   按q退出
   
7: head 查看文件的前几行
   -n 7查看前7行 
   
8: tail 查看文件末位的几行
   -n 7查看末位的7行
   -f 动态显示文件末尾内容
   
9: chmod 修改文件的权限 (所有者和 root 可以修改)
   + 增加权限，- 减少权限，= 指定权限 
   chmod {ugoa} {+-=} {rwx} u:所有者,g:所属组,o:其他, a:所有
   权限数字: r:4, w:2, x:1 如：rwxrw-r-- : 764
   例子：chmod 776 /etc/hosts 
   -R 递归修改
   (目录w权限) 可以删除和创建文件, 注意: 文件的w权限只有写权限
   
10: chown 修改文件的所有者
    chown zhangxx ./a.txt 将当前目录的文件a.txt的所有者改成zhangxx
	
11: chgrp 修改文件的所属组
    chown 组名 目录名或者文件名
	
12: find 查找文件
	-iname 根据文件名查找 不区分大小写
	-name 根据文件名查找,区分大小写
	-size 文件在大小, +大于多少 -小于多少 如: find /etc -size +2600(这个数据块) 1kb=2个数据块
	-user 查找所有者的文件
	-group 查找所属组的文件
	-mmin -5 查找文件内容5分钟内被修改过的文件 +超过多少时间，-在多少时间内
	-type f(文件) d(目录) l(软链接) 
	-exec 查找后做什么事情如：find /etc -name *init -exec ll {} \;   /*其中{} \; 是固定格式 */
	      查看文件后而且显示出文件的详细信息
	
	-o 表示多个条件or 满足其中一个就可以
	-a 表示多个条件合在一起(and) 如：find /etc -size +2603 -a -size -45612389 文件大小在某个区间内的文件
	
    find /etc -name init  在/etc文件目录下查找文件名和init的文件(精确查找)
	find /etc -name *init* 只要文件名中包含init的就返回 *匹配多个字符
    find /etc -name ???init ?匹配单个字符
	

