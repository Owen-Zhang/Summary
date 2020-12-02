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

```



