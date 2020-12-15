##### 约定
``` 
版本号 就是sha1码(4fec36f)
```

###### git放弃本地文件修改
``` sh
#1: 未使用git add 缓存代码 
   git checkout . #可以返原已修改的内容, 但新增的文件不起作用，不会自动删除
   git checkout -- filename #放弃某个文件(filename:文件名), 可以多个文件一起,用空格隔开就可以了

#2: 已使用git add 缓存代码，未使用git commit  
   git reset HEAD filename # 将某个提交commit 的文件返回到 git add 之前
   git reset HEAD #将所有提交commit的文件返回到git add 之前
   #返回到git add 之前，要将修改丢掉就用git checkout

#3: 已经用 git commit 提交了代码
   git reset head~1 #将上一次提交的代码返回到git add 之前(此常用), 1可以是n
   git reset --soft head~1 #返回到上次git add 之后，git commit 之前的状态
   git reset --hard head~1 #直接返回到上次commit的版本

   #可以跳转到任务一个版本(就是那个sha1码)
   git reset --hard 版本号 #一次性回退到某个版本(commit之后的版本)
   git resset 版本号 #返回到某个版本号未add 之前的编辑状态
```

##### 2日志内容查看
``` sh
git log -p #可以看详细的修改内容, 其中 按 【j】向下，按【k】向上 看 【q】退出
git log --oneline #只显示提交的版本号和说明,比较实用
```

##### 3 打tag
``` sh
git tag -a v1 -m "正式版本"  版本号 #给程序打标签， 版本号可要可不要，加上就表示在哪个提交后打的标签，不加就表示当前 如: git tag -a v1 -m "正式版" 4sfasf23

git tag -d v1 #删除v1 tag

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
git branch 分支名 已存在的版本号 #可以在某个版本的时刻创建分支

git checkout  dev #切换分支, 如果切换不存在的分支会报错,切换分支前最好将当前分支的代码提交
git checkout 版本号 #可以切换到某个版本号 ---这个要注意
git checkout -b mybranch #创建并切换分支(如果分支不存在就创建), mybranch分支名
git checkout -b 本地分支名 origin/远程分支名 #从远程拉取新的分支并切换到新的分支(将会自动创建一个新的本地分支，并与指定的远程分支关联起来)

git branch -d 分支名 # 删除本地分支,但有一个前提必须先git branch 到其它分支,如果有未合并的代码时会报错
git branch -D 分支名 # 删除本地分支,但有一个前提必须先git branch 到其它分支,不管是否有未合并的代码直接删除
git push origin --delete 分支名  #删除远程分支
```

##### 6 合并分支
``` sh
git checkout master #切换到要合并到的目的地分支
git merge dev       #将当前的dev分支合并到当前分支(master)上
```

##### 7 将本地仓库和远程仓库关联
``` sh
git init test #本地创建test文件夹并加上git相关的初始化信息
git remote add origin http://github.com/Owen-Zhang/test.git #将本地仓库和线上仓库关联,origin是git默认远程标识,也可以改成其它
```

##### 8 将本地commit后的代码推到远程
``` sh
##### git push <远程主机名> <本地分支名>:<远程分支名> ###### 远程主机名:origin
git push origin master:master #标准push方式：origin:远程主机名 master(第一个):本地分支名,master(第二个):远程分支名
git push origin master #如果省略远程分支名，则表示将本地分支推送与之存在"追踪关系"的远程分支（通常两者同名），如果该远程分支不存在，则会被新建
git push origin # 如果当前分支与远程分支之间存在追踪关系，则本地分支和远程分支都可以省略。
git push #如果当前分支只有一个追踪分支，那么主机名都可以省略。

git push -u origin master #如果当前分支与多个主机存在追踪关系，则可以使用-u选项指定一个默认主机，这样后面就可以不加任何参数使用git push。
```

##### 9 .gitignore忽略文件
``` sh
* #任意字符
*.property #排除以property结尾的文件
!b.property #包含b.property文件
dir/ #dir目录下面的所有文件目录都排除
dir/*.txt #排除dir目录下面(一级目录)的txt文件
dir/*/*.txt #排除dir目录下第三级目录里txt文件
dir/**/*.txt #排除dir目录下任意目录下的txt文件

```

##### 10 拉取
``` sh
git pull origin master:localbranch #从远程master分支拉取到本地localbranch, 如果localbranch不存在就创建
```

##### 11 git commit 和 git add 一起用
``` sh
#1 git add . && git commit -m "test" && git push origin dev:dev
#2 git commit -am "test" && git push origin dev:dev 这个用要注意
```

##### 12 查看所有的提交版本,不管是否回退或者分支回并什么的，都可以看
``` sh
git reflog #看到所有的版本号与注释，可以git reset --hard 后面的版本号
           #这个命令很重要
```


##### 13 分支重命名
``` sh
git branch -m master master2 #将master分支的名字从master改为master2
```

##### 14保存现场
``` sh
#代码在没有提交(commit)的情况下是不允许切换分支(两个分支没有在同一个阶段)
#如果还没有将一个功能开发完成就要切换分支：建议 1保存现场(stash), 2切换

git stash save "现场的名称" #保存现场(git commit 之前)
git checkout master #切换到其它分支做其它事情
git checkout dev #又要回到原来的分支
git stash list #查看保存的现场列表(当有多个现场保存时)
git stash pop #恢复第一个保存的现场(而且是恢复到工作区，没git add .之前)

```

##### 15 查看某个文件的提交情况
``` sh
 git blame .\git\git相关命令.md  #git blame 文件名
```


##### 16 ssh 配制，提交代码不用输入用户名和密码
``` sh
#本地私钥，远程公钥
ssh-keygen #在bash中生成私钥,windows默认会生成在用户->当前登陆用户名-.ssh里
           #会生成两个文件，一个公钥，一个私钥, github.com -> setting -> ssh and gpg keys里
           #【new ssh key】将公钥放在此处

git clone git@github.com:Owen-Zhang/Summary.git #clone代码时就用ssh的方式
git push origin master:master #提交时就不用输入用户名和密码了(相当于私钥加密，公钥解密)
```

##### 17 查看远程地址、分支、以及当前head对应的分支
``` sh
git remote show origin 
   #* remote origin
   #Fetch URL: git@github.com:Owen-Zhang/Summary.git
   #Push  URL: git@github.com:Owen-Zhang/Summary.git
   #HEAD branch: master
   #Remote branches:
   #   dev    tracked
   #   master tracked
   #Local branch configured for 'git pull':
   #   master merges with remote master
   #Local ref configured for 'git push':
   #   master pushes to master (up to date)
```

##### 18 查看本地和远程分支情况
``` sh
git branch -av #
```