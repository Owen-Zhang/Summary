 var cultureCookie = HttpContext.Current.Request.Headers["mps-Language"];

var cultureName = !string.IsNullOrEmpty(cultureCookie) ? cultureCookie : "en-us";

switch (cultureName.ToLower())
{
    case "zh-cn":
    case "zh-tw":
        break;

    default:
        cultureName = "en-us";
        break;
}

Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(cultureName);

//怎么生成resx 和 design.cs文件请见console程序下面的generateRes类
