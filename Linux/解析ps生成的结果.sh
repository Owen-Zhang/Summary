ps -ef | grep mysql | while read line
do
	name=${line:0:5}
	if [ $name == "mysql" ]; then 
		arr=(${line// / }) #以空格分开 
		#echo ${arr[1]}
		kill -9 ${arr[1]} #如果是以服务的方式运行的话，结束了又会启动的 systemctl stop mysqld才行
	fi
done
