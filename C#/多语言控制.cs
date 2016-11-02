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
