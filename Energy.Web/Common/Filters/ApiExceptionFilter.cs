using Energy.Web.Common;
using Energy.Web.Common.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Energy.Web.Common.Filters;

/// <summary>
/// Giden HttpClient işleyici zinciri tarafından fırlatılan API kimlik doğrulama
/// istisnalarını doğru istemci yanıtına dönüştürür:
/// <list type="bullet">
/// <item>Tam sayfa gezinmeleri, giriş / erişim reddedildi ekranına klasik bir 302
/// yönlendirmesi alır.</item>
/// <item>AJAX/JSON istekleri (DevExtreme grid'leri, fetch yardımcıları), eşleşen
/// 401/403 durumuyla birlikte <c>{ redirect }</c> JSON zarfı alır; istemci tarafındaki
/// <c>AppHttp</c> katmanı bunu bir bildirim + yönlendirmeye dönüştürür. Burada 302
/// üretmek işe yaramazdı: <c>fetch</c> onu şeffaf şekilde izler ve grid bir HTML
/// sayfasını JSON olarak ayrıştırmada başarısız olur.</item>
/// </list>
/// </summary>
public sealed class ApiExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    /// <summary>Günlükleyiciyi enjekte eder.</summary>
    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    /// <summary>API kimlik doğrulama istisnalarını yakalayıp uygun yönlendirmeyi üretir.</summary>
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var request = context.HttpContext.Request;
        var currentPath = request.Path + request.QueryString;

        switch (context.Exception)
        {
            case ApiUnauthorizedException:
                _logger.LogWarning(context.Exception,
                    "API rejected request as unauthorized (401) for {Path}.", currentPath);
                // Çerez yerel olarak kabul edildi ancak içindeki JWT API tarafından
                // reddedildi (süresi dolmuş, imzalama anahtarı/güvenlik damgası
                // uyuşmazlığı, ...). Kullanıcının, API'nin asla kabul etmeyeceği bir
                // jetonla artık "oturum açmış" görünmemesi için çerezi düşür ve onu
                // giriş sayfasına gönder.
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
                // API işlemi 403 ile reddetti. Erişim reddedildi ekranında gerçek
                // istenen yolu göster (belirli yetki kodu yalnızca API tarafında
                // bilindiğinden sayfa varsayılanına bırakılır).
                var deniedUrl = "/account/access-denied?path=" + Uri.EscapeDataString(currentPath);
                context.Result = request.WantsJson()
                    ? new JsonResult(new { redirect = deniedUrl }) { StatusCode = StatusCodes.Status403Forbidden }
                    : new RedirectToActionResult("AccessDenied", "Account", new { path = currentPath });
                context.ExceptionHandled = true;
                break;
        }
    }
}
