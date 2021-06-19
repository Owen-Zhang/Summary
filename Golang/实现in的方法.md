``` go
func In(haystack interface{}, needle interface{}) (bool, error) {
    sVal := reflect.ValueOf(haystack)
    kind := sVal.Kind()
    if kind == reflect.Slice || kind == reflect.Array {
        for i := 0; i < sVal.Len(); i++ {
            if sVal.Index(i).Interface() == needle {
                return true, nil
            }
        }

        return false, nil
    }

    return false, ErrUnSupportHaystack
}

// sVal.Index(i).Interface() == needle 数组/slice中不能有 == 比较不了的，只支持基本类型

```


