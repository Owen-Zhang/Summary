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

##### #############################################################

#### 1 将一个命令的执行结果赋给变量
```sh
A=`ls -la` #反引号，运行里面的命令，并把结果返回给变量A
A=$(ls -la) #等价于反引号 eg:  aa=$((4+5))
bb=`expr 4 + 5 ` #数字要运算就加上expr
```

##### 2 将一个变量赋给另一个变量
　  eg : A=$STR

##### 3 显示当前日期
　　echo `date +%Y%m%d`　//20200611 注意Y是大写,+前面有空格,后面没有空格, %Y中间可以加-
　　#也可以只输出`date +%Y` 2020(只输出年)

##### 4 awk 可以输出某一列(字段)
   ifconfig | grep broadcast | awk '{print $6}' #列数是从1开始

##### 5 sed 替换内容等(还有很多有用的信息)
   ff="adddd:456232563" echo $ff | sed 's/adddd://g'  #将adddd:替换成空  sed 's/adddd:/+/g' 将adddd:替换成+
   ll | awk '{print $9}' | grep -v '^$' #输出当前目录下的文件名(去除空行)
   sed 's/^/& /' #在行前添加空格
   
##### 6 grep相关操作
	-i 忽略大小写
	-o 只输出匹配到的部分(而不是整个行)
	-v 反向选择，即输出没有没有匹配的行  grep -v '^[a-zA-Z].*' #输出不是以字母开始的行
	-c 计算找到的符号行的次数
	-n 顺便输出行号

##### 7 if 双括号法
   if [[ $score == 0 || ($score > 4 && $score < 8) ]]; then
   
##### 8 符号$后的括号
	#: ${a} 变量a的值, 在不引起歧义的情况下可以省略大括号
	#: $((exp)) 和`expr exp`效果相同, 计算数学表达式exp的数值,三目运算符和逻辑表达式都可以计算
	#: (()) 增强括号的用法, 常用于算术运算比较. 双括号中的变量可以不使用$符号前缀, 只要括号中的表达式符合C语言运算规则, 支持多个表达式用逗号分开.
	#: 比如可以直接使用for((i=0;i<5;i++)), 如果不使用双括号, 则为for i in `seq 0 4`或者for i in {0..4}.
	#: 再如可以直接使用if (($i<5)), 如果不使用双括号, 则为if [[ $i < 5 ]]
	
##### 9 多条命令执行
	#: (cmd1;cmd2;cmd3) 新开一个子shell顺序执行命令cmd1,cmd2,cmd3, 各命令之间用分号隔开, 最后一个命令后可以没有分号.
	#: { cmd1;cmd2;cmd3;} 在当前shell顺序执行命令cmd1,cmd2,cmd3, 各命令之间用分号隔开, 最后一个命令后必须有分号, 第一条命令和左括号之间必须用空格隔开.
	#: 对{}和()而言, 括号中的重定向符只影响该条命令, 而括号外的重定向符影响到括号中的所有命令.
	
##### 10 数组
``` sh
A=(test1 test2 test3) #定义用空格分隔
B=("12" "test" "ddfdsfs") #定义数组
echo ${A[1]} # 输出test2
arr=(`ls /root`); echo ${arr[1]};  #通过ls /root生成新的数组 
unset arr[1] #删除第二个元素
echo ${arr[@]:1:2} #从第二个元素开始,截取2个值
echo ${lsArr[@]:1} #从第二个元素开始到最后一个值

#遍历数组
for(( i=0;i<${#arr[@]};i++))
do
echo ${arr[i]};
done

#遍历数组
for path in ${arr[@]}
do
echo $path
done

#其它说明: ${arr} 获取数组中第一个值
#         ${#arr[*或者@]} 数组中元素的个数
#         ${arr[*或者@]} 引用数组中所有的元素
#         ${#arr} 数组中下标为0的字符个数       
```

##### 11 关联数组(相当于map/hash)
``` sh
declare -A myDic=(["name"]=test ["age"]=18) #声明并赋值
echo ${mydic["name"]} #获取某个key的值
echo ${mydic[*或者@]} #获取所有的数据
echo ${#mydic[*或者@]} #获取数据的个数
```
##### 12 流编辑 sed
``` sh
sed s/root/root1/g passwd #将passwd文件中的root替换成root1 /g是全局替换 passwd表示文件名
sed -n 's/root/root1/g w change' passwd # -n + w 将修改过的结果写到change文件中,同时change文件必须存在，否则会报错
sed -r 's/^root/root1/g' passwd #使用正则表达式来替换
sed -n '2,12 s/root/root1/g w change' passwd #从第2行到12行进行替换,将12换成$号表示到最后一行
sed -i "s/nologin1/nologin/g" passwd #将passwd文件中的nologin1改为nologin
```

##### 13 grep 文件内容查找
``` sh
grep -i "hello" /etc/passwd #查找passwd文件中包含hello的数据行, 不区分大小写,可以同时查询多个文件，用空格隔
开就可以了
grep -n "hello" /etc/passwd #查找passwd文件中包含hello的数据行,并显示行号
grep -o 					#只显示匹配到的内容
grep -v                     #不包括某些规则的数据
```

##### 14 curl 访问网络数据
``` sh
curl -s http://www.baidu.com | grep -o "京ICP证[0-9]*" | grep -o "[0-9]*" #-s:不显示统计信息; -o:只显示匹配到的结果

```

##### 15 awk使用
``` sh
ll `which awk` #显示which awk所在路径的信息

```




