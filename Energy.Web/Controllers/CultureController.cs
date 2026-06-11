using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers;

[AllowAnonymous]
[Route("culture")]
public sealed class CultureController : Controller
{
    [HttpGet("set")]
    public IActionResult Set(string culture, string uiCulture, string? returnUrl = null)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, uiCulture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });
        return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
    }
}
