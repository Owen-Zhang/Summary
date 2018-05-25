package main

import "runtime"
import "fmt"

func main() {
	runtime.GOMAXPROCS(1)

	go func() {
		fmt.Println("test other")
	}()

	go func() {
		for i := 0; i < 10; i++ {
			fmt.Println(i)
		}
	}()

	fmt.Println("begin")
	for {
		runtime.Gosched()
	}

	fmt.Println("end")
}
