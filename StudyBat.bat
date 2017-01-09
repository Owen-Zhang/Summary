@echo off 

::for 的参数，在bat文件中以%%i, 在cmd(命令行中%i)

:: set str= c d e f g h i j k l m n o p q r s t u v w x y z 
:: echo  当前硬盘的分区有： 
:: for %%i in (%str%) do if exist %%i: echo %%i;


:: for /d 操作目录(只操作一级目录，不会递归查找)，/f 操作文件, /r 递归的查找当前目录和下级目录
::输出当前目录 pushd调整当前目录，popd 返回当前目录 for 输出当目录中的所有*.cs文件 ^
::echo %cd%
::pushd D:\code\ConsoleTest\src\Model
::for  %%i in (*.cs) do echo "%%i"
::popd 
::echo %cd%


::输出一个文件的内容,如果不指定"delims="就表示以空格或跳格作为分隔符号, 每行只读到设置的符号就停止
::"符号列表"而非"符号",可以一次性指定多个分隔符号
::pushd D:\code\ConsoleTest\src\Model
::for /f "delims=" %%i in (ShipMethod.cs) do echo %%i


::skip= 结合 delims= 去除前6行，从第7行开始读数据
::pushd D:\code\ConsoleTest\src\Model
::for /f "delims= skip=6" %%i in (ShipMethod.cs) do echo %%i

::eol= 表示为不以什么字符打头(下面：不以空格打头的行)
::pushd D:\code\ConsoleTest\src\Model
::for /f "delims= eol= " %%i in (ShipMethod.cs) do echo %%i 

::for /f %%i in (文件名) do („„)  如果文件名中含有空格或&时，应使用引号把路径括起来
::for /f %%i in ('命令语句') do („„) 
::for /f %%i in ("字符串") do („„) 
::for /f "usebackq" %%i in ("文件名") do („„) 为了兼容文件名中所带的空格或&
::for /f "usebackq" %%i in (`命令语句`) do („„) 
::for /f "usebackq" %%i in ('字符串') do („„) 

::查询D盘下所有以exe结尾的程序
::for /r D:\ %%i in (*.exe) do echo %%i

::输出当前目录中以exe结尾的文件
::pushd D:\code\ConsoleTest\src
::for /r %%i in (*.exe) do echo %%i

::%ERRORLEVEL% 系统 返回上一条命令的错误代码。通常用非零值表示错误

::FOR /L %%variable IN (start,step,end) DO command [command-parameters]
::for /L %%i in (1, 1, 6) do echo %%i
::输出 1 2 3 4 5 6

:: 一个bat文件中调用另一个bat文件，以及参数的输入
:: 主bat如下:
::call test2.bat %USERPROFILE% %cd%
::echo main program
::---------------------
::子bat文件如下
::@echo off
::echo come from other bat file
::echo %1
::echo %2
::echo %0  --test2.bat

:: 自定义参数和使用参数 set /p 可以让用户输入参数值
::set var= test my var: 
::echo %var%

::判断某两个字符串是否相等，用 if "字符串1"=="字符串2" 会区分大小写
:: if /i "字符串1"=="字符串2" 的格式 这个是不区分大小写的 
::判断某两个数值是否相等，用 if 数值1 equ 数值2 语句； 
::判断某个变量是否已经被赋值，用 if defined str 语句
::判断上个命令的返回值，if errorlevel 数值 command
::判断驱动器，文件或文件夹是否存在，if exist filename command 语句
::if exist d:\123.bat (echo 123.bat文件存在！) else echo 123.bat文件不存在！ 

::set a=
::if defined a (echo 变量 a 已定义) else (echo 变量 a 没有被定义)
::当我们用if defined 变量 command 语句判断变量是否被定义时，请注意 变量 为不使用引导符号%的变量名，不能用写为%变量%，否则出错。
::等于 equ equal 
::大于 gtr greater than 
::大于或等于 geq greater than or equal 
::小于 lss less than 
::小于或等于 leq less than or equal 
::不等于 neq no equal

::删除当前目录下的test.txt
::if exist test.txt (del test.txt) else (echo don't exits)

::从输出从1到10 如果%%i后面加上>nul 表示不显示
::for /L %%i in (1, 1, 10) do (echo %%i)

:: 标志的使用(:Label, goto label就可以跳转到那)
:: set /a 指定等号右边的字符串为被评估的数字表达式,就是表示计算,如：set /a z=123+456; 表示为z=579

::使用set赋值时参数和=间不能有空格
::set a= %path%
::echo %a%

::截取功能统一语法格式为：%a:~[m[,n]]%
::%为变量标识符，a为变量名，不可少，冒号用于分隔变量名和说明部分，符号～可以简单理解为“偏移”即可，m为偏移量（缺省为0），n为截取长度
::set word=abscdefg
::echo %word:~1,10%

::循环调用, 调用完之后再次调用文件
::（a.bat内容如下)
::@echo off
::if exist C:\Progra~1\Tencent\AD\*.gif del C:\Progra~1\Tencent\AD\*.gif
::a.bat


::---http://blog.sina.com.cn/s/blog_4ce992f40102w0o0.html : 使用批处理脚本查是否中冰河-----------------------------------
pause 
