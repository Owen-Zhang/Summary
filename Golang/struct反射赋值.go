import (
	"fmt"
	"reflect"
)

type A struct {
	Name    string
	Address string
}

func GetReflectInfo() {
	a := []A{}

	value := reflect.ValueOf(&a)

	fmt.Println(value.Kind() == reflect.Struct)
	fmt.Println(value.Elem().Kind() == reflect.Struct)
	fmt.Println(value.Elem().Kind() == reflect.Slice)

	ty := reflect.TypeOf(a).Elem()
	//v := reflect.New(ty)
	v := reflect.New(ty).Elem()
	for i := 0; i < ty.NumField(); i++ {
		f := v.Field(i)
		switch f.Kind() {
		case reflect.String:
			f.Set(reflect.ValueOf("test"))
		}
	}
	fmt.Println(v)

	// fmt.Println(reflect.New(ty))
	// fmt.Println(reflect.New(ty).Elem())

	//fmt.Println(reflect.TypeOf(a).Elem())
}


//相关资料
https://www.jianshu.com/p/76c3171857a7
https://blog.csdn.net/qq317808023/article/details/50192897
https://www.cnblogs.com/mmdsnb/p/6439267.html
