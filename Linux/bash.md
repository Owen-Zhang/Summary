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
``` sh
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

   #实例 查找某个进程并kill掉
   ps -ef | grep mysql | grep -v grep | while read line
   do
      name=${line:0:5}
      if [ $name == "mysql" ]; then 
         arr=(${line// / }) #以空格分开 
         #echo ${arr[1]}
         kill -9 ${arr[1]} #如果是以服务的方式运行的话，结束了又会启动的 systemctl stop mysqld才行
      fi
   done

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
``` sh
   ff="adddd:456232563" echo $ff | sed 's/adddd://g'  #将adddd:替换成空  sed 's/adddd:/+/g' 将adddd:替换成+
   ll | awk '{print $9}' | grep -v '^$' #输出当前目录下的文件名(去除空行)
   sed 's/^/& /' #在行前添加空格
```

##### 7 if 双括号法
```
   if [[ $score == 0 || ($score > 4 && $score < 8) ]]; then
```

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
	-c 计算找到的符号行的次数
   grep -v '^[a-zA-Z].*' #输出不是以字母开始的行
   ps aux | grep mysql | grep -v grep  #找出mysql进程相关信息
   grep -i "hello" /etc/passwd #查找passwd文件中包含hello的数据行, 不区分大小写,可以同时查询多个文件，用空格隔
   开就可以了
   grep -n "hello" /etc/passwd #查找passwd文件中包含hello的数据行,并显示行号
   grep -o 					#只输出匹配到的部分(而不是整个行)
   grep -v              #反向选择，即输出没有没有匹配的行
```

##### 14 curl 访问网络数据
``` sh
curl -s http://www.baidu.com | grep -o "京ICP证[0-9]*" | grep -o "[0-9]*" #-s:不显示统计信息; -o:只显示匹配到的结果

curl https://www.baidu.com #发出get请求

#-b 参数 发送cookie
curl -b 'add=c;fff=g' https://www.baidu.com #多个cookie用;分隔
curl -b t.txt https://www.baidu.com #将一个文件中的cookie传入

#-c 参数 保存response cookie
curl -c t.txt https://www.baidu.com #将服务器返回的cookie保存到t.txt中

#-d 参数 post提交数据
curl -d 'ddd=dfff&g=dddfgg' https://www.baidu.com # post提交相关参数
curl -d '@t.txt' https://www.baidu.com # 提交一个文件的内容到服务器
$ curl -d '{"login": "emma", "pass": "123"}' -H 'Content-Type: application/json' https://google.com/login

#--data-urlencode 参数 urlencode编码提交数据
curl --data-urlencode 'ddd=dfff&g=dddfgg' https://www.baidu.com # post提交相关参数

#-e 参数用来设置 HTTP 的标头Referer，表示请求的来源。
curl -e 'http://www.sina.com' https://www.baidu.com 

#-F 参数用来向服务器上传二进制文件。
curl -F 'file=@t.png' https://www.baidu.com # 
上面命令会给 HTTP 请求加上标头Content-Type: multipart/form-data，然后将文件photo.png作为file字段上传
```

##### 15 awk使用
``` sh
ll `which awk` #显示which awk所在路径的信息

```

##### 16 ll/ls 列出文件和目录信息
``` sh
   -a 列出所有的文件
   -h 文件大小显示出 k, m 
   -i 文件数字标识
   -d 只显示指定目录的信息
```

##### 17 mkdir 创建目录
```
   -p 递归创建目录，上级目录不存在就创建
```

##### 18 cp 复制文件或者目录
``` sh
   -r 复制目录
   -f 如果有相同就覆盖
   -p 保持文件的属性不变
   cp ./a.txt ./b.txt /root 将a.txt和b.txt复制到root目录下(复制多个文件)
   cp 过去还可以改名
   \cp -rf /aaa /test  #将aaa目录复制到/test目录下,存在就覆盖覆盖
```

##### 19 mv 移动文件或者改名

##### 20 cat 查看文件
``` sh
   -n 可以看到行号
   cat -n /etc/passwd | more #显示行号并且分页显示
```

##### 21 less 查看文件内容 
```  sh
   less /etc/services # 会一屏一屏的显示, 对显示内容特别的文件特别好
   可以向下看，向上看
   还可以查找 /a  查找a字符 按n(next)表示查看下一次查找的位置
   按q退出
```

##### 22 head 查看文件的前几行
``` sh
   -n 7 #查看前7行 
   head -n 8 /etc/passwd # 查看文件的前8行
```

##### 23 tail 查看文件末位的几行
``` sh
   -n 7 #查看末位的7行
   -f   #动态显示文件末尾内容
```

##### 24 chmod 修改文件的权限 (所有者和 root 可以修改)
```
   + 增加权限，- 减少权限，= 指定权限 
   chmod {ugoa} {+-=} {rwx} u:所有者,g:所属组,o:其他, a:所有
   权限数字: r:4, w:2, x:1 如：rwxrw-r-- : 764
   例子：chmod 776 /etc/hosts 
   -R 递归修改
   (目录w权限) 可以删除和创建文件, 注意: 文件的w权限只有写权限
```

##### 25 chown 修改文件的所有者
```
   chown zhangxx ./a.txt 将当前目录的文件a.txt的所有者改成zhangxx
```

##### 26 chgrp 修改文件的所属组
```
   chgrp 组名 目录名或者文件名
```

##### 27 find 查找文件
``` sh
-iname 根据文件名查找 不区分大小写
	-name 根据文件名查找,区分大小写 # find / -name mysql 在全文件中查找mysql相关的文件或文件夹
	-size 文件在大小, +大于多少 -小于多少 如: find /etc -size +2600(这个数据块) 1kb=2个数据块
	-user 查找所有者的文件
	-group 查找所属组的文件
	-mmin -5 查找文件内容5分钟内被修改过的文件 +超过多少时间，-在多少时间内
	-type f(文件) d(目录) l(软链接) 
	-exec 查找后做什么事情如：find /etc -name *init -exec ll {} \;   /*其中{} \; 是固定格式 */
	查看文件后而且显示出文件的详细信息
	-o 表示多个条件or 满足其中一个就可以
	-a 表示多个条件合在一起(and) 如：find /etc -size +2603 -a -size -45612389 文件大小在某个区间内的文件
	
    find /etc -name init  #在/etc文件目录下查找文件名和init的文件(精确查找)
	 find /etc -name *init* #只要文件名中包含init的就返回 *匹配多个字符
    find /etc -name ???init #?匹配单个字符
```

##### 28 locate 快速查找
```
    -i 不区分大小写
    locate init* 查找以init开头的文件
    更新文件数据库 updatedb， 不然新建的文件查找不到
    不会查找tmp文件夹下面的文件
```

##### 29 which, whereis 查找命令
``` sh
which docker  #查找docker的可运行程序路径,如果有别名也会显示出来 
whereis docker #查找命令相关的一些路径和帮助信息
```

##### 30 w 
```
可以查看当前在线的用户
```

##### 31  ping
``` sh
ping -c 3 www.baidu.com #-c 发几次包 
```

##### 32  last
```
所有用户登陆的时间信息以及重启时间
```

##### 33  traceroute
```
显示数据包到达主机的路径
```

##### 34 netstat 查看系统的网络连接状态、路由信息、接口
``` sh
    -atlunp 查看本机开启的端口情况(活动的),以及数据发送情况
    -an 查看本机所有的网络连接
    -rn 查询路由列表
    -a: 显示所有连线中的Socket,已建立的连接（ESTABLISHED），也包括监听连接请（LISTENING）
    -n : 表示不将ip和端口转换成域名和服务名
    -t : 查看tcp相关信息
    -u : 查看udp相关信息
    -p : 显示pid和进程名
    -l : 监听
    -s: 能够按照各个协议分别显示其统计数据
    -e: 本选项用于显示关于以太网的统计数据。它列出的项目包括传送的数据报的总字节数、错误数、删除数、数据报的数量和广播的数量。这些统计数据既有发送的数据报数量，也有接收的数据报数量。这个选项可以用来统计一些基本的网络流量
```

##### 35 service network restart 重启网络

##### 36 vim 相关命令
``` sh
    命令模式:
    :set nu 加上行号
    gg 到第一行
    G  到最后一行
    :n 到某一行
    $　移动行尾
    0  移动到行首
    x 删除光标所在处的字符
    dd 删除光标所在行,也是剪切光标所在行
    D 删除光标到行尾
    :100,115d 删除100行到115行的数据
    yy 复制当前行的数据
    p 粘贴数据，经常和yy结合使用
    2yy 复制2行数据
    ndd 剪切多行数据
    u 取消上一次操作

    /string 搜索要找的字符串
    n 找下一个要查找的结果

    :set ic 不区分大小写
    :set noic区分大小写
    :r /etc/hosts 将/etc/hosts中的内容导入到当前文件中
    :!whereis docker 在不退出当前vim的情况下，运行其它命令
    :r !ls -l /etc 将后面命令运行的结果写入当前文件中

    :1,6s/^/#/g 将1到6行注解掉
    :1,6s/^#//g 将1行到6行的以#号开头的#号去掉, 不加^就会去掉行中所有的#, 说白了就是替换
```

##### 37 touch 
``` sh 
   touch {1..100}.txt #创建 1.txt 到 100.txt文件
```

##### 38 查看centos 版本
``` sh
cat /etc/redhat-release
```

##### 39 查看linux内核信息
``` sh
uname -a # 显示系统名、节点名称、操作系统的发行版号、内核版本等等
```

##### 40 系统日志
``` sh
/var/log #下面有很多的日志信息，如messages等
```

##### 41 tcpdump 抓取tcp信息
``` sh
tcpdump -nn -i ens33 port 80 #抓取ens33网卡上,端口为80的流入流出数据
#port 指定端口
#-i 监视指定网络接口的数据包 如果不指定网卡，默认tcpdump只会监视第一个网络接口
#-X(大写) 显示交互的内容

tcpdump host baidu ## host只抓取某个host的包数据(也可以用ip)
tcpdump -i eth0 src host hostname ## 抓取hostname发出的数据包(发出)
tcpdump -i eth0 dst host hostname ## 抓取发到hostname的数据包(接收)
tcpdump tcp port 23 and host 210.27.48.1 ## 可以用and 或者or关联多个条件, tcp/udp 指抓取哪种类型数据
```

##### 42 设置dns服务器
``` sh
#路径 /etc/resolv.conf 增加相应的dns服务器
nameserver 192.168.106.189
nameserver 192.168.106.190
```

##### 43 配制ip、防火墙
```
路径： /etc/sysconfig/network-scripts/ #ip
      /etc/sysconfig/iptables-config  #防火墙
```

##### 44 service 开机启动程序设置
``` sh
#1: 服务一般放在 /usr/lib/systemd/system目录，也可能在/etc/systemd/system
#2: 新增一个文件如: aa.service 内容如下
#aa.service文件内容如下：
[Unit]
Description=mgrweb_sso
Requires=
After=

[Service]
EnvironmentFile=~/.bashrc
PIDFile=/var/run/mgrweb_sso.pid
ExecStartPre=rm -f /var/run/mgrweb_sso.pid
WorkingDirectory=/home/sso_v3/mgrserver/bin
ExecStart=/home/sso_v3/mgrserver/bin/mgrweb_sso run -r zk://192.168.0.101 -c v3
Restart=on-failure
RestartSec=50s

[Install]
WantedBy=multi-user.target

#3 设置开机运行: systemctl enable aa
#4 启动服务: systemctl start aa
#5 停止服务: systemctl stop aa
#6 直接停止运行: systemctl kill aa.service
#7 重启服务：  systemctl restart aa

#8: [Service] 区块：启动行为
ExecReload字段：重启服务时执行的命令
ExecStop字段：停止服务时执行的命令
ExecStartPre字段：启动服务之前执行的命令
ExecStartPost字段：启动服务之后执行的命令
ExecStopPost字段：停止服务之后执行的命令
```

##### 45 shell命令行参数的相关说明
``` sh
$# 是传给脚本的参数个数
$0 是脚本本身的文件名
$1 是脚本后接的第一个参数
$2 是脚本后接的第二个参数
$@ 是传给脚本的所有参数列表，"$1" "$2" "$3" … "$n"
$* 是以一个单字符串显示传给脚本的所有参数，"$1 $2 $3 … $n"
$$ 是脚本运行的当前进程ID号
$? 是最后运行命令的结束状态码，0表示没有错误，其他表示有错误

shift 造成参数变量号码偏移，第二个参数变为$1，以此类推。

#echo $@ 多个参数时 动态判断
 for i in $@
 do  
 if [ $i = "oci" ]; then 
 	echo "oci"
 fi;
 if [ $i = "prod" ]; then 
 	echo "prod"
 fi
 done  

```

##### 46 ubuntu更新vscode
``` sh
#1 先下载最新版本(ubuntu 下载 *.deb) 下载文件为: c.deb

#2 安装更新 
sudo dpkg -i c.deb #c.deb文件名
```

##### 47 命令行时将光标定位到行首、行末
``` sh
ctrl + A # 行首
ctrl + E #行末
```

##### 48 chmod（修改文件权限）
``` sh
chmod +x 文件名 #给文件增加运行权限(对所有用户有效)
chmod -x 文件名 #给文件减少运行权限(对所有用户有效)
chmod +r 文件名 #给文件增加读权限(对所有用户有效)
chmode 755 文件名 # r:读取权限,数字代号为“4”; w:写入权限,数字代号为“2”；x:执行或切换权限，数字代号为“1”; -:不具任何权限，数字代号为“0”

who
u:用户
g:组
o:其它
a:所有用户(默认)

chmod u=rwx,g=rx,o=x 文件名 #给【u】用户读写运行文件的权限; 给【组】读和运行权限；给【其它】运行权限

```

##### 49 df 文件系统上的大小、使用空间和可用空间
``` sh
df -h  #大小、使用空间和可用空间
```

##### 50 free 查看内存使用总体情况
``` sh
free -h
#               total        used        free      shared  buff/cache   available
# Mem:           2.9G        646M        1.3G        9.1M        1.0G        2.0G
# Swap:            0B          0B          0B
``` 

##### 51 groups 查看用户属于哪个组
``` sh
groups huige #显示huige属于哪个组
```

##### 52 history
``` sh
history  #查看历史shell命令
!数字    #数字表示那一行的编号,相当于运行那个命令 
!!      #运行上一个命令
```

##### 53 kill 杀死进程
``` sh
kill -9 进程id  #强制杀死进程
``` 

##### 54 sh scp 运程连接与远程传输文件
``` sh
ssh root@192.168.0.110 #通过root连接到110机器
scp 11.conf root@192.168.0.110:/root/test/ #将11.conf文件复制到110机器上的root/test目录下 
```

##### 55 top 查看进程信息
``` sh
top  #可以查看所有进程的内存和cpu使用情况
``` 

##### 56 查看内核版本信息
``` sh
cat /proc/version 
# Linux version 3.10.0-693.el7.x86_64 (builder@kbuilder.dev.centos.org) (gcc version 4.8.5 20150623 (Red Hat 4.8.5-16) (GCC) ) #1 SMP Tue Aug 22 21:09:27 UTC 2017

cat /etc/redhat-release #查看redhat版本信息
```




