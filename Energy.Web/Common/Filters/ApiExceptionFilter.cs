using Energy.Web.Common.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Energy.Web.Common.Filters;

/// <summary>
/// Converts the auth-related exceptions thrown by
/// <c>AuthHeaderHandler</c> into the right user-facing redirect or JSON
/// response, depending on whether the request is an AJAX call from DevExtreme
/// or a normal browser navigation.
/// </summary>
public sealed class ApiExceptionFilter : IAsyncExceptionFilter
{
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ApiUnauthorizedException:
                await HandleUnauthorizedAsync(context);
                break;

            case ApiForbiddenException:
                HandleForbidden(context);
                break;
        }
    }

    private static async Task HandleUnauthorizedAsync(ExceptionContext context)
    {
        // Drop the stale cookie so the next request lands on /account/login.
        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        var loginUrl = "/account/login?returnUrl=" + Uri.EscapeDataString(BuildReturnUrl(context.HttpContext.Request));

        if (IsAjax(context.HttpContext.Request))
        {
            context.Result = new ObjectResult(new
            {
                redirect = loginUrl,
                reason = "session_expired"
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
        else
        {
            context.Result = new RedirectResult(loginUrl);
        }

        context.ExceptionHandled = true;
    }

    private static void HandleForbidden(ExceptionContext context)
    {
        var requestedPath = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
        var redirectUrl = "/account/access-denied?path=" + Uri.EscapeDataString(string.IsNullOrWhiteSpace(requestedPath) ? "/" : requestedPath);

        if (IsAjax(context.HttpContext.Request))
        {
            context.Result = new ObjectResult(new
            {
                redirect = redirectUrl,
                reason = "forbidden"
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
        else
        {
            context.Result = new RedirectResult(redirectUrl);
        }

        context.ExceptionHandled = true;
    }

    private static bool IsAjax(HttpRequest request)
    {
        if (string.Equals(request.Headers.XRequestedWith, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var accept = request.Headers.Accept.ToString();
        return accept.Contains("application/json", StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildReturnUrl(HttpRequest request)
    {
        if (HttpMethods.IsGet(request.Method))
        {
            return request.PathBase + request.Path + request.QueryString;
        }

        // POST/PUT/DELETE etc. — sending the user back to the same URL on a
        // GET makes no sense; default to the dashboard root.
        return "/";
    }
}

