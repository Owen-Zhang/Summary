//处理提交的参数信息

// use map as struct
    var clusterinfo = url.Values{}
    //var clusterinfo = map[string]string{}
    clusterinfo.Add("userName", user)
    clusterinfo.Add("password", pw)
    clusterinfo.Add("cloudName", clustername)
    clusterinfo.Add("masterIp", masterip)
    clusterinfo.Add("cacrt", string(caCrt))

    data := clusterinfo.Encode()
    
    url := "https://10.10.105.124:8443/user/checkAndUpdate"
    reqest, err := http.NewRequest("POST", url, strings.NewReader(data))
