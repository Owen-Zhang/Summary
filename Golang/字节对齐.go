type                                  size in bytes
byte, uint8, int8                     1
uint16, int16                         2
uint32, int32, float32                4
uint64, int64, float64, complex64     8
complex128                            16
struct{}, [0]T{}                      0

arrary                                由其原素(element)的类型决定
struct                                由其字段(field)的类型决定
other types                           一个机器字的大小


例子
type Age struct {
 arr [2]int8  //2两个字节
 bl  bool     //1个字节
 //此处会padding 5个字节 
  
  sl []int16  //24个字节
  ptr *int64  //8个字节 指针在64位系统占8个字节,32位系统中占4个字节
  str  string //16个字节
  map[string]int16  //8个字节
  i  interface{}  //16个字节
}
