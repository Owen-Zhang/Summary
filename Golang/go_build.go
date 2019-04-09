golang 编译生成可执行文件

cd ~/work/src/TaskServer
env GOOS=linux GOARCH=amd64 go build -o ../bin/
