package encoding

import "fmt"
import "encoding/json"

type Order struct {
	OrderNumber int32 	 `json:"ordernumber"`
	Price float64	  	 `json:"price"`
	CustomerNumber int32 `json:"ctomernumber"`
	CustomerName string  `json:"customername"`
}

func testJson() {
  orderInfo := &Order {
		OrderNumber : 123,
		Price : 125.36,
		CustomerNumber : 785,
		CustomerName : "AlTop",
	}
	result, _ := json.Marshal(orderInfo)
	fmt.Println(string(result))
  
  //json.HTMLEscape()
}

	/*result:
		{"ordernumber":123,"price":125.36,"ctomernumber":785,"customername":"AlTop"}
	*/
