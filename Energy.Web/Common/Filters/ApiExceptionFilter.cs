using Energy.Web.Common.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Energy.Web.Common.Filters;

public sealed class ApiExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ApiUnauthorizedException:
                // The cookie was accepted locally but the JWT inside it was
                // rejected by the API (expired, signing-key/security-stamp
                // mismatch, ...). Drop the cookie so the user is no longer
                // "signed in" with a token the API will never honour, then
                // send them to the login page.
                if (context.HttpContext.User.Identity?.IsAuthenticated == true)
                {
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }

                var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl });
                context.ExceptionHandled = true;
                break;
            case ApiForbiddenException:
                context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                context.ExceptionHandled = true;
                break;
        }
    }
}
