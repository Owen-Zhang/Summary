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