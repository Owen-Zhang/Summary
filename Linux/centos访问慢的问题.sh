centos ipv4,ipv6解析域名时有冲突，需在/etc/resolv.conf里加一句options timeout:2 attempts:2 rotate single-request-reopen 
