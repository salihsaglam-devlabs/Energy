using Energy.Web.Common;
using Energy.Web.Common.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Energy.Web.Common.Filters;

/// <summary>
/// Converts the API auth exceptions raised by the outbound HttpClient handler
/// chain into the right client response:
/// <list type="bullet">
/// <item>Full-page navigations get a classic 302 redirect to the login /
/// access-denied screen.</item>
/// <item>AJAX/JSON requests (DevExtreme grids, fetch helpers) get a JSON
/// envelope <c>{ redirect }</c> with the matching 401/403 status, which the
/// client-side <c>AppHttp</c> layer turns into a notification + redirect.
/// Emitting a 302 here would be useless: <c>fetch</c> follows it transparently
/// and the grid then fails parsing an HTML page as JSON.</item>
/// </list>
/// </summary>
public sealed class ApiExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var request = context.HttpContext.Request;
        var currentPath = request.Path + request.QueryString;

        switch (context.Exception)
        {
            case ApiUnauthorizedException:
                _logger.LogWarning(context.Exception,
                    "API rejected request as unauthorized (401) for {Path}.", currentPath);
                // The cookie was accepted locally but the JWT inside it was
                // rejected by the API (expired, signing-key/security-stamp
                // mismatch, ...). Drop the cookie so the user is no longer
                // "signed in" with a token the API will never honour, then
                // send them to the login page.
                if (context.HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }

                var loginUrl = "/account/login?returnUrl=" + Uri.EscapeDataString(currentPath);
                context.Result = request.WantsJson()
                    ? new JsonResult(new { redirect = loginUrl }) { StatusCode = StatusCodes.Status401Unauthorized }
                    : new RedirectToActionResult("Login", "Account", new { returnUrl = currentPath });
                context.ExceptionHandled = true;
                break;

            case ApiForbiddenException:
                _logger.LogWarning(context.Exception,
                    "API rejected operation as forbidden (403) for {Path}.", currentPath);
                // The API rejected the operation with 403. Surface the real
                // requested path on the access-denied screen (the specific
                // permission code is only known API-side, so it is left to the
                // page default).
                var deniedUrl = "/account/access-denied?path=" + Uri.EscapeDataString(currentPath);
                context.Result = request.WantsJson()
                    ? new JsonResult(new { redirect = deniedUrl }) { StatusCode = StatusCodes.Status403Forbidden }
                    : new RedirectToActionResult("AccessDenied", "Account", new { path = currentPath });
                context.ExceptionHandled = true;
                break;
        }
    }
}
