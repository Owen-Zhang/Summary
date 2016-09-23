// You can edit this code!
// Click here and start typing.
package main

import "fmt"
import "regexp"

func main() {
	reg, _ := regexp.Compile(`\w+`)
	fmt.Println(reg.FindAllString("sdf\nddd/asdfas", -1))
	reg1, _ := regexp.Compile(`^[0-9]*[1-9][0-9]*$`)
	if flag := reg1.MatchString("545468"); flag {
		fmt.Println("Input is right")
	}
	reg2, _ := regexp.Compile(`\W`)
	fmt.Println(reg2.ReplaceAllString(`addd\sasdf\d_+sdf8787!`, "@"))

	reg3, _ := regexp.Compile(`@`)
	fmt.Println(reg3.Split("@@sdf@545@!!@@54568", -1))
	fmt.Println(len(reg3.Split("@@sdf@545@!!@@54568", -1)))

	/*result:
		[sdf ddd asdfas]
		Input is right
		addd@sasdf@d_@sdf8787@
		[  sdf 545 !!  54568]
		7
	*/
}
