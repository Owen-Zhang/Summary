#!/bin/bash

echo "准备结束apiserver"
pida=$(ps -ef | grep "/home/17sup/apiserver/apiserver" | grep -v "grep"|awk '{print $2}') 
#如果找到就结束进程
if [ $? -eq 0 ]; then
    echo "process id:$pida"
   
    echo "结束进程 apiserver" $pida
    kill -9 $pida
fi

echo "准备结束flowserver"
pidf=$(ps -ef | grep "/home/17sup/flowserver/flowserver" | grep -v "grep"|awk '{print $2}') 
if [ $? -eq 0 ]; then
    echo "process id:$pidf"
   
    echo "结束进程 flowserver" $pidf
    kill -9 $pidf
fi


echo "备份文件 apiserver"
cd /home/17sup/apiserver
time=$(date "+%Y-%m-%d-%H:%M:%S")
mv apiserver "apiserver-${time}"

echo "备份文件 flowserver"
cd /home/17sup/flowserver
time=$(date "+%Y-%m-%d-%H:%M:%S")
mv flowserver "flowserver-${time}"


echo "复制文件到运行目录"
cd /home/17sup_temp
mv ./apiserver /home/17sup/apiserver/
mv ./flowserver /home/17sup/flowserver/


echo "运行程序 apiserver"
cd /home/17sup/apiserver
nohup /home/17sup/apiserver/apiserver run -r zk://192.168.0.101 -c apiserver &
echo "-----------------------------"
tail -n 50 nohup
echo "-----------------------------"

echo "运行程序 flowserver"
cd /home/17sup/flowserver
nohup /home/17sup/flowserver/flowserver run -r zk://192.168.0.101 -c flowserver &
echo "-----------------------------"
tail -n 50 nohup
echo "-----------------------------"
