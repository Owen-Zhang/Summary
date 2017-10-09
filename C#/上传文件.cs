public static UploadFileViewModel UploadFile(HttpContext context, string appName)
        {
            var collection = context.Request.Form;
            string fileKey = context.Request.Form["filekey"];
            HttpPostedFile file = context.Request.Files[fileKey];


            var path = ConstValue.FileBaseUrl + "/";
            if (Nesoft.ECWeb.Entity.ConstValue.HaveSSLWebsite)
            {
                path = ConstValue.FileBaseUrlSSL + "/";
            }
            if (file != null && file.ContentLength > 0)
            {
                path += "UploadHandler.ashx?appName=" + appName;
            }

            string result = string.Empty;
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest requst = (HttpWebRequest)WebRequest.Create(path);
            requst.ContentType = "multipart/form-data; boundary=" + boundary;
            requst.Method = "POST";
            requst.KeepAlive = true;
            requst.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = requst.GetRequestStream();

            if (collection != null && collection.Keys.Count > 0)
            {
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (string key in collection.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    string formitem = string.Format(formdataTemplate, key, collection[key]);
                    byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
            }

            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, "uploadfile", file.FileName, "application/octet-stream");
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);


            byte[] buffer = new byte[file.InputStream.Length];
            int bytesRead = 0;
            while ((bytesRead = file.InputStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse response = null;
            try
            {
                response = requst.GetResponse();
                Stream stream2 = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream2);
                result = reader.ReadToEnd();
            }
            catch
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }
            }


            UploadFileViewModel model = new UploadFileViewModel()
            {
                IsSuccess = false
            };

            if (!string.IsNullOrEmpty(result))
            {
                dynamic resultToken = DynamicJson.Parse(result);
                if (resultToken.IsDefined("state"))
                {
                    model.IsSuccess = (resultToken["state"] == "SUCCESS");
                }
                if (resultToken.IsDefined("url"))
                {
                    model.FileUrl = resultToken["url"];
                }
                if (resultToken.IsDefined("message"))
                {
                    model.Message = resultToken["message"];
                }
            }
            return model;
        }
