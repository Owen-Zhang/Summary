@echo off 

::for 的参数，在bat文件中以%%i, 在cmd(命令行中%i)

:: set str= c d e f g h i j k l m n o p q r s t u v w x y z 
:: echo  当前硬盘的分区有： 
:: for %%i in (%str%) do if exist %%i: echo %%i;


:: for /d 操作目录，/f 操作文件
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

pause 
