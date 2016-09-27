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
