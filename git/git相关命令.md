###### git放弃本地文件修改
```
1: 未使用git add 缓存代码 
   git checkout . 可以返原已修改的内容, 但新增的文件不起作用，不会自动删除
   git checkout -- filename 放弃某个文件(filename:文件名), 可以多个文件一起,用空格隔开就可以了

2: 已使用git add 缓存代码，未使用git commit  
   git reset HEAD filename -- 将某个提交commit 的文件返回到 git add 之前
   git reset HEAD --将所有提交commit的文件返回到git add 之前
   返回到git add 之前，要将修改丢掉就用git checkout

3: 已经用 git commit 提交了代码
   git reset head~1 将上一次提交的代码返回到git add 之前(此常用)
   git reset --soft head~1 将上一次提交的代码返回到git add 之后，git commit 之前的状态
   git reset --hard head~1 将上一次提交的代码全部删除,直接返回到修改之前的代码状态(此方法慎用)
```

##### 2日志内容查看
``` sh
git log -p #可以看详细的修改内容, 其中 按 【j】向下，按【k】向上 看 【q】退出
```