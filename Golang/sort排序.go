sort.go包中有很多像 type StringSlice []string  type Float64Slice []float64 ... 类型的slice，可以拿来就用

func KeysOfMap(m map[string]string) []string {
	keys := make(sort.StringSlice, len(m))
	i := 0
	for key := range m {
		keys[i] = key
		i++
	}

	keys.Sort()
	return []string(keys)
}
