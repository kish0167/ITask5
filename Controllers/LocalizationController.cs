using System.Collections.Specialized;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ITask5.Controllers;

public class LocalizationController : Controller
{
    public IActionResult Index(string culture)
    {
        SetLanguage(culture);
        string returnUrl = UpdateLanguageQueryParameter(Request.Headers.Referer.ToString(), culture);
        return Redirect(returnUrl);
    }
    
    private void SetLanguage(string culture)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions {Expires = DateTimeOffset.UtcNow.AddYears(1)}
        );
    }
    
    private string UpdateLanguageQueryParameter(string oldUrl, string cultureQueryParameter)
    {
        Uri uri = new Uri(oldUrl, UriKind.RelativeOrAbsolute);
        NameValueCollection query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        query["language"] = cultureQueryParameter;
        string path = uri.IsAbsoluteUri ? uri.AbsolutePath : uri.OriginalString.Split('?')[0];
        string queryString = query.ToString() ?? "";
        return queryString.Length > 0 ? $"{path}?{queryString}" : path;
    }
}