##### bash相关的知识点
```
1: ctrl + A 移动光标到行首
2: ctrl + E 移动光标到行末
3: ctrl + U 删除光标之前的命令
4: ctrl + K 删除光标之后的命令
5: ctrl + S 暂停屏幕的输出
6: ctrl + Q 恢复屏幕的输出
```

##### 后台作业
```
1: 命令(command) + &  (命令会在后台运行)
2: ctrl + Z 将当前作业切换到后台(这个不会运行,stop状态)
   eg: 如用vim编辑一个文件时，可以先将其放到后台, 通过jobs查看后台作业状态
3: jobs 查看后台作业状态
4: fg %n 让后台运行的作业n切换到前台来
5: bg %n 让指定的作业n在后台运行
6: kill %n 移除指定的作业n
```

##### 其它
```
1: ll -h /root &>/dev/null 将正确和错误的信息输出到垃圾回收站
   0 输入、1 正确输入、2 错误输出 
   echo "aaaa" > aa.txt 正确输出
   echo "err" 2> bb.txt 错误输出
   &> 1.txt 将正确和错误都输出到1.txt文件中

2: wc统计行数
   wc -l < readme.md  #统计readme.md的行数
   还可以使用管道(|)来统计行数

3: while 使用
   while read str; do
     echo $str
   done < readme.md
```

##### 命令链接符
```
1: && 并且(前面正常了才运行后面)
2: || 或者(前面错误了才会运行后面,前面正确了就不运行后面的命令)

```

##### shell脚本调试
```
-n 不运行脚本,仅检查语法问题
-v 在运行脚本前,先将脚本输出到屏幕上
-x 将使用的脚本输出到屏幕上

sh -x test.sh
```


##### 变量
```
1: 定义变量 #变量名必须以字母或_开始,区分大小写
   dir=$(pwd) 获取当前路径,并赋值给dir变量   
2: 获取变量值
   echo $dir 获取变量的值

3: 位置变量 #运行程序后面的值
   # ./test.sh 1 2 3 (后面的参数要以空格分开)
   # 文件中可以使用$1 $2 $3 表示三个值

4: 预定义变量
   ./test.sh 1 2 3
   $0 脚本名称 #test.sh
   $@ 所有参数 #1 2 3 
   $# 参数个数 #3
   $$ 当前进程的pid #

5: 定义引用变量
   "" 弱引用
   '' 强引用
   "${aa}" #获取aa的值
   '${aa}' #直接输出${aa}
   反引号(``)等价于$() 如: echo `date +%F` ==> echo $(date +%F)

6: 变量运算
   整数运算: 
      expr  + - \* / %
      $(())  #eg: echo $((1 + 1))  + - * / % 
      $[]    #eg: echo $[1+1]  + - * / %
      let    #let: let sum=1+2; echo $sum
   小数运算: 结合 |bc
      加法: echo "123.23 + 456.78" |bc  #580.01
      减法: echo "123.23 - 456.78" |bc  #-333.55
      乘法: echo "123.23 * 456.78" |bc  #scale是控制小数位数,这里情况很多
      除法: echo "scale=2;123.23 * 456.78" |bc #scale是控制小数位数,这里情况很多 

7: 变量内容
   获取变量内容的长度: url=test.com; echo ${#url}
   获取变量的一部分内容: url=test.com echo ${url:0:4}  #test,0:start; 4:count;
   替换内容: url=test.com; echo ${url/test/bbb} #bbb.com
            url=test.com; echo ${url//t/T} #TesT.com 两个//就是贪婪匹配
   分隔字符成数组:
     line="mysql 1764 1 0 15:56 ? 00:00:00 /usr/sbin/mysqld --daemonize --pid-file=/var/run/mysqld/mysqld.pid"
     arr=(${line// / }) #以空格字符,其中最后一个/后面的空格不能去
     echo ${arr[1]}     #取数组第二个值 1764

     line2="1,2,3,4,5,6";
     arr2=(${line2//,/ }); # 以(,)分隔,}前面的空格不能少
     echo ${arr2[1]}       # 2
     
     ###注意下面这种方式
     echo ${line// /}   #这个是替换，就是将字符中的空格去掉而已

```

##### 函数
```
1： 定义
    function name {
       commands
       [return value]
    }
    name() {
       commands
       [return value]
    }

2: 调用函数
   name 1 2 ##函数中使用$1 $2使用参数(函数将结果标准输出)
   result=$? 获取上次运行的输出值
```

##### 循环结构
```
1: for 语法结构
   for i in {1..10} #在/root目录下创建10个demo目录
   do
   mkdir -p /root/demo$i
   done  

   #######################
   dir="/var" #显示/var目录下目录占用的磁盘大小
   cd $dir
   for i in $( ll $dir)
   do
      [ -d $i ] && du -sh $i
   done

2: while 语法结构
   while read demo
   do
      echo $demo
   done </etc/password

   ########################
   a=1
   sum=0
   while ((a <=9))
   do
      let sum+=a;
      let ++a
   done
   echo $sum

```
