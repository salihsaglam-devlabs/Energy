using Energy.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Energy.Web.Common.Filters;

/// <summary>
/// HTML ekranları için sayfa düzeyinde erişimi uygular. <see cref="PagePermissionAttribute"/>
/// ile işaretlenmiş bir controller/eylem, yalnızca oturum açmış kullanıcı eşleşen yetki
/// talebine (claim) sahipse erişilebilir. Kimlik doğrulamanın kendisi çerez yedek
/// politikasıyla yönetilir; bu filtre yalnızca yetki kapısını ekler; böylece yetkisiz
/// bir kullanıcı, veri çağrıları sonradan 403 ile başarısız olan boş bir ekran görmek
/// yerine erişim reddedildi sayfasına yönlendirilir. Her gerçek veri işlemi için API
/// tarafı yetkilendirme doğruluk kaynağı olmaya devam eder.
/// </summary>
public sealed class PageAccessFilter : IAsyncAuthorizationFilter
{
    /// <summary>Sayfa yetki kapısını uygular; yetkisizleri erişim reddedildi sayfasına yönlendirir.</summary>
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // Anonim sayfalar (giriş, erişim reddedildi, ...) asla kapı altına alınmaz.
        if (context.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any())
        {
            return Task.CompletedTask;
        }

        var attribute = context.ActionDescriptor.EndpointMetadata
            .OfType<PagePermissionAttribute>()
            .LastOrDefault();

        if (attribute is null)
        {
            return Task.CompletedTask;
        }

        var user = context.HttpContext.User;

        // Kimliği doğrulanmamış istekler çerez sınamasına (challenge) bırakılır
        // (/account/login'e yönlendirme); biz yalnızca gerçekten bir kullanıcı
        // mevcut olduğunda yetki kapısını ekleriz.
        if (user.Identity?.IsAuthenticated != true)
        {
            return Task.CompletedTask;
        }

        if (!user.HasPermission(attribute.Permission))
        {
            // Gerçek istenen yolu + yetki kodunu erişim reddedildi ekranına taşı;
            // böylece yer tutucu varsayılanlar yerine eyleme dönük "erişim talep et"
            // ayrıntılarını gösterebilir.
            var request = context.HttpContext.Request;
            var deniedUrl = "/account/access-denied"
                + "?path=" + Uri.EscapeDataString(request.Path + request.QueryString)
                + "&permission=" + Uri.EscapeDataString(attribute.Permission);

            // AJAX/JSON çağıranlar makine tarafından okunabilir bir yönlendirme zarfı
            // alır; tam sayfa gezinmeleri erişim reddedildi ekranına klasik bir 302 alır.
            context.Result = request.WantsJson()
                ? new JsonResult(new { redirect = deniedUrl }) { StatusCode = StatusCodes.Status403Forbidden }
                : new RedirectToActionResult("AccessDenied", "Account", new
                {
                    path = request.Path + request.QueryString,
                    permission = attribute.Permission
                });
        }

        return Task.CompletedTask;
    }
}
