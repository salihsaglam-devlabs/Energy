using Energy.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Controllers;

[AllowAnonymous]
[Route("culture")]
public sealed class CultureController : Controller
{
    [HttpGet("set")]
    public IActionResult Set(string culture, string? uiCulture = null, string? returnUrl = null)
    {
        var requestedCulture = string.IsNullOrWhiteSpace(culture)
            ? CultureConstants.DefaultCulture
            : culture;

        var requestedUiCulture = string.IsNullOrWhiteSpace(uiCulture)
            ? requestedCulture
            : uiCulture;

        var supported = CultureConstants.SupportedCultures
            .Any(item => string.Equals(item.Name, requestedCulture, StringComparison.OrdinalIgnoreCase));

        if (!supported)
        {
            requestedCulture = CultureConstants.DefaultCulture;
            requestedUiCulture = CultureConstants.DefaultCulture;
        }

        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(requestedCulture, requestedUiCulture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                HttpOnly = false,
                SameSite = SameSiteMode.Lax,
                Secure = Request.IsHttps
            });

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return LocalRedirect("/");
    }
}

