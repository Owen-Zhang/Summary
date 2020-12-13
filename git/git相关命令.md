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
git log --oneline #只显示提交的版本号和说明,比较实用
```

##### 3 打tag
``` sh
git tag -a v1 -m "正式版本"  版本号 #给程序打标签， 版本号可要可不要，加上就表示在哪个提交后打的标签，不加就表示当前 如: git tag -a v1 -m "正式版" 4sfasf23

git push origin v1.0 # push单个tag，命令格式为：git push origin [tagname]
git push --tags #push所有tag
```

##### 4 show 查看相关新增/修改的具体内容,也可以看tag信息
``` sh
git show 版本号 #版本号: 是git log 里面很长的那一段字符
```

#### 5 查看远程分支/创建分支/切换分支/删除本地分支/删除远程分支
``` sh
git branch -r  #查看远程分支
git branch dev #创建分支,如果存在会报错
git checkout  dev #切换分支, 如果切换不存在的分支会报错
git checkout 版本号 #可以切换到某个版本号 ---这个要注意
git checkout -b mybranch #创建并切换分支(如果分支不存在就创建), mybranch分支名
git branch -D 分支名 # 删除本地分支,但有一个前提必须先git branch 到其它分支
git push origin --delete 分支名  #删除远程分支
```

aaaaa