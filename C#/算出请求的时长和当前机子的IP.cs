protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items["X-REQ-BEGINTIME"] = DateTime.Now;
        }
        
 protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (!HttpContext.Current.Items.Contains("X-REQ-BEGINTIME"))
                return;
            var begin = (DateTime)HttpContext.Current.Items["X-REQ-BEGINTIME"];
            HttpContext.Current.Items["X-REQ-BEGINTIME"] = null;
            var elapsed = DateTime.Now - begin;
            if (elapsed.TotalMilliseconds > 5000)
            {
                if (HttpContext.Current == null)
                {
                    return;
                }

                var req = HttpContext.Current.Request;
                var url = req.Url.AbsolutePath;

                object body = string.Empty;
                if (!req.ContentType.Contains("multipart/form-data"))
                {
                    var reader = new StreamReader(req.InputStream);
                    body = reader.ReadToEnd();
                    req.InputStream.Position = 0;
                }
                else
                {
                    body = req.InputStream.Length;
                }

                _performancelogger.WarningFormat(@"Low performance request, elapsed Time:{0}
                            Url   : {1}
                            Header: {2}
                            Body  : {3}
                    ", elapsed, url, HttpUtility.UrlDecode(req.Headers.ToString()), body);
            }
        }
        
        得到当前机子的IP 地址
        --------------------------------------
        var hostName = System.Net.Dns.GetHostName();
        string ipAddress = string.Empty;
            try
            {
                var addressList = System.Net.Dns.GetHostEntry(hostName).AddressList;
                foreach (var address in addressList)
                {
                    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddress = address.ToString();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                ipAddress = string.Empty;
            }
        
