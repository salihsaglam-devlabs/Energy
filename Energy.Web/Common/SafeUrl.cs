using Microsoft.AspNetCore.Mvc;

namespace Energy.Web.Common;

/// <summary>
/// Açık yönlendirme (open-redirect) güvenlik açıklarını önlemek için kullanıcı
/// tarafından verilen <c>returnUrl</c> değerlerini güvenle ele alan yardımcılar.
/// </summary>
public static class SafeUrl
{
    /// <summary>returnUrl yerel ve geçerliyse onu, aksi halde verilen yedek (fallback) yolu döndürür.</summary>
    public static string GetLocalReturnUrl(this IUrlHelper url, string? returnUrl, string fallback)
    {
        if (!string.IsNullOrEmpty(returnUrl) && url.IsLocalUrl(returnUrl))
        {
            return returnUrl;
        }

        return fallback;
    }
}

