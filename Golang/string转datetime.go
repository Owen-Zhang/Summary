const Layout = "2006-01-02" //时间常量
	loc, _ := time.LoadLocation("Asia/Shanghai")
	time1, _ := time.ParseInLocation(Layout, "2019-10-21", loc)
	if time1.Sub(time.Now()) < 0 {
		fmt.Println("太小了")
		return
	}
