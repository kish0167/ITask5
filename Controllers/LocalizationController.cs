using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace ITask5.Controllers;

public class LocalizationController : Controller
{
    public IActionResult Index(string culture)
    {
        SetLanguage(culture);
        string returnUrl = Request.Headers.Referer.ToString();
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
}