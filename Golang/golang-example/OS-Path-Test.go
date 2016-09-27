package OS-Test

import "fmt"
import "os"
import "os/exec"
import "path/filepath"

func Test() {

    //创建文件
    f, _ := os.Create("123.txt")
	defer f.Close()
	//APPend 内容
    f.WriteString("defer f.Close()")

	fmt.Println("File Successfull")
	//程序运行路径
    exePath, _ := exec.LookPath(os.Args[0])
	//运行的目录
    foldPath := filepath.Dir(exePath)
	//拼接路径
    newJoinPath := filepath.Join(foldPath, "\\11\\22.txt")
	//分离路径和运行程序名
    splitFold, fileName := filepath.Split(exePath)

	/*根据相对位置去读取*/
	f2, err := os.OpenFile("147.txt", os.O_RDWR, 0777) //此处可以加上flag，来控制打开要作的动作，Append， Write等
	if err != nil {
		fmt.Println(err)
	}
	defer f2.Close()
	position,_ := f2.Seek(2, 1)
	buffer := make([]byte, 8) 
	returnCount,_ := f2.ReadAt(buffer, position)
	fmt.Println(string(buffer[:returnCount-1]))
	fmt.Println(returnCount)
	/*第二次读取，一般会在for里面去处理，要注意byte[]重新初始化，不然会有脏数据*/
	buffer = make([]byte, 8)
	returnCount, _ = f2.ReadAt(buffer, position + int64(returnCount))
	conventStr := string(buffer[:returnCount-1])
	fmt.Println(conventStr)
	fmt.Println(returnCount)
	//f2.WriteString("sdfasfasdf2121\n") sdfasfasdf2121

	//查看当前的工作目录路径 得到测试文件的绝对路径
	current_dir, _ := os.Getwd()
	fmt.Println(current_dir)

	fmt.Println(exePath)
	fmt.Println(foldPath)
	fmt.Println(newJoinPath)
	fmt.Println(splitFold, "@@@@@", fileName)
}

/*
result:
    File Successfull
    C:\Users\oz3t\AppData\Local\Temp\go-build365055090\command-line-arguments\_obj\exe\main.exe
    C:\Users\oz3t\AppData\Local\Temp\go-build365055090\command-line-arguments\_obj\exe
    C:\Users\oz3t\AppData\Local\Temp\go-build365055090\command-line-arguments\_obj\exe\11\22.txt
    C:\Users\oz3t\AppData\Local\Temp\go-build365055090\command-line-arguments\_obj\exe\ @@@@@ main.exe
*/
