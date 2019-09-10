#1: 服务一般放在 /usr/lib/systemd/system目录，也可能在/etc/systemd/system
#2: 新增一个文件如: aa.service 内容如下
###
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
###

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
