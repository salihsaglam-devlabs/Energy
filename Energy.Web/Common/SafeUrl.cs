using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Common;

/// <summary>
/// Helpers for safely handling user-supplied <c>returnUrl</c> values to prevent
/// open-redirect vulnerabilities.
/// </summary>
public static class SafeUrl
{
    public static string GetLocalReturnUrl(this IUrlHelper url, string? returnUrl, string fallback)
    {
        if (!string.IsNullOrEmpty(returnUrl) && url.IsLocalUrl(returnUrl))
        {
            return returnUrl;
        }

        return fallback;
    }
}

