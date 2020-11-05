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
4: grep 使用
   grep -v 反向查找
   ps aux | grep mysql | grep -v grep  #找出mysql进程相关信息
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

3: 循环控制
   break n    #n表示跳出循环的次数,如果省略n表示跳出整个循环
   continue n #n退到n层继续循环，如果省略n表示路过本次循环进入下一次循环
   exit n     #退出当前的shell程序,并返回n
   return     #函数中用到
```

##### 流程控制
```
1: 符号说明
   (()) 数值比较、运算
   [[]] 条件测试、支持正则
   $(()) 整数运算
   $[] 整数运算
   
2: 文件测试表达式
   -d 文件存在并且为目录 # [ -d /root ]; [ ! -d /root ] 判断目录是否存在
   -f 文件存在并且为普通文件
   -e 文件存在,不管是目录还是普通文件
   -s 文件存在并且大小不为0
   -r 文件存在并且可读; -w可写; -x可运行 -L链接文件 

3: 整数比较操作符
   [] 和 test    ||         [[]] 和 (())
      -eq                      == =
      -nq                      !=
      -gt                      >
      -ge                      >=
      -lt                      <
      -le                      <=
```

##### 分支语句if
```
1: 单分支
if [ 条件表达式 ]; then 
   业务处理
fi

2: 双分支
if [ 条件表达式 ]; then 
   业务处理
else
   业务处理
fi

3:多分支语句
if [ 条件表达 ]; then
   业务处理
elif [ 条件表达式]; then
   业务处理
else
   业务处理
fi

#######################
查找某个进程的pid
#######################
mysql=$(ps aux | grep mysql | grep -v grep)
if [[ -n $mysql ]]; then
  name=${mysql:0:5}
  if [ $name == "mysql" ]; then
   arr=(${mysql// / })
   echo ${arr[1]}
  fi
else
  echo “mysql not exists”
fi
############################## 
```