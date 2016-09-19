//字节数
var str string = "abc张"  //32位电脑上,共6个字节
charray := []byte(str)    //转换为byte:一个字节对应一个数组元素
fmt.Println(len(charray)) //获取元素的个数,结果为6
//可以直接用 len(str) 也是6

//字符数
i := "abc张"
fmt.Println(len([]rune(i))) // 3个
 
