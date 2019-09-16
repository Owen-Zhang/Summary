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
