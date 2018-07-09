package main

import "fmt"
import "reflect"

type aa struct {
	C string `root:"abcdefg" other:"other1"`
	D string `root:"qwert" other:"other2"`
}

func main() {
	ins := &aa{C: "cccc", D: "147"}
	typ := reflect.TypeOf(ins).Elem()
	val := reflect.ValueOf(ins).Elem()

    for i:= 0; i < typ.NumField(); i++ {
        //field := typ.Field(i)
        if val.Field(i).Kind() == reflect.String {
            val.Field(i).SetString("123456")
        }
        //fmt.Println(field.Name)
        //fmt.Println(field.Tag.Get("other") + "\n")
    }
    fmt.Println("-------------------")
    
    fmt.Println(val)
    fmt.Println(ins)
}
