//去掉末尾多余的0
decimal g = 10.01M;
Console.WriteLine(g.ToString("#0.##"));

StringBuilder sb = new StringBuilder();
sb.AppendFormat("{0}={1}", cookieName, cookieValue);
sb.AppendFormat(";domain={0}", parameters["domain"]);
sb.AppendFormat(";path={0}", parameters["path"]);
//配置为0，则cookie在会话结束后失效
if (expires > 0)
{
    // 用max-age取代expires 使用相对时间来作为有效期
    sb.AppendFormat(";max-age={0}", Convert.ToString(expires * 60));
}

HttpContext.Current.Response.AddHeader("Set-Cookie", sb.ToString());


//也可以用HttpContext.Current.Response.cookies.add(new cookie());

//http://blog.csdn.net/zhangxinrun/article/details/6427369
//http://www.cnblogs.com/rookie-26/p/4663899.html
