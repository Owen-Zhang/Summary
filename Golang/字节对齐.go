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
