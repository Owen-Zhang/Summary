1.声明为声明为map[string]struct{}

由于struct{}是空，不关心内容，这样map便改造为set

map可以通过“comma ok”机制来获取该key是否存在,例如_, ok := map["key"],如果没有对应的值,ok为false

可以通过定义成map[string]struct{}的形式,值不再占用内存。其值仅有两种状态，有或无

如果定义的是map[string]bool，则结果有true、false或没有

 

下面的例子用于查看数组中是否有重复的值，就使用了map[string]struct{}

如果任何值在数组中出现至少两次，函数返回 true。如果数组中每个元素都不相同，则返回 false。

func containsDuplicate(nums []int) bool {
    m := make(map[int]struct{})
    for _, v := range nums{
        _, ok := m[v]
        if ok {
            return true
        }
        m[v] = struct{}{}
    }
    return false
}
 

2.chan struct{}：可以用作通道的退出
3.两个structt{}{}地址相等
