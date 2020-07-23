#### curl 相关使用方法

##### 1.  curl https://www.baidu.com
发出get请求

##### 2. -b 参数 发送cookie
```
curl -b 'add=c;fff=g' https://www.baidu.com #多个cookie用;分隔
curl -b t.txt https://www.baidu.com #将一个文件中的cookie传入
```

##### 3. -c 参数 保存response cookie
```
curl -c t.txt https://www.baidu.com #将服务器返回的cookie保存到t.txt中
```

##### 4. -d 参数 post提交数据
```
curl -d 'ddd=dfff&g=dddfgg' https://www.baidu.com # post提交相关参数
curl -d '@t.txt' https://www.baidu.com # 提交一个文件的内容到服务器
$ curl -d '{"login": "emma", "pass": "123"}' -H 'Content-Type: application/json' https://google.com/login
```

##### 5. --data-urlencode 参数 urlencode编码提交数据
```
curl --data-urlencode 'ddd=dfff&g=dddfgg' https://www.baidu.com # post提交相关参数
```

##### 6. 参数用来设置 HTTP 的标头Referer，表示请求的来源。
```
curl -e 'http://www.sina.com' https://www.baidu.com # 
```

##### 6. -F 参数用来向服务器上传二进制文件。
```
curl -F 'file=@t.png' https://www.baidu.com # 
上面命令会给 HTTP 请求加上标头Content-Type: multipart/form-data，然后将文件photo.png作为file字段上传
```

##### 7 -H 设置header
