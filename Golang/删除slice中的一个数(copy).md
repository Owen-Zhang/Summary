##### copy的灵活使用

``` go
//通过copy删除slice中某个(或者多个)数据
pconns := []string{"a", "中文", "英文", "good"}
fmt.Println(pconns)
copy(pconns[2:], pconns[2+1:]) //(将英文去掉),把第四个数开始的后面的数据复制到第三个数
pconns = pconns[:len(pconns)-1] //原来的数据就多出一个，将最后一个数去除
fmt.Println(pconns)
```
