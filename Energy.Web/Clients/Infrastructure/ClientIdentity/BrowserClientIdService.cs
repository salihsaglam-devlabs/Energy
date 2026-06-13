namespace Energy.Web.Clients.Infrastructure.ClientIdentity;

/// <summary>
/// Bir HTTP çerezi aracılığıyla tarayıcı başına bir ilişkilendirme (correlation) kimliği
/// üretip kalıcı hale getirir; böylece API, istekleri belirli bir istemciye (kimliği
/// doğrulanmış kullanıcıdan ayrı olarak) atfedebilir. HTTP bağlamı yokken (örn. arka plan
/// görevleri) sunucu makinesi önekli bir kimliğe geri düşer.
/// </summary>
public sealed class BrowserClientIdService
{
    private const string CookieName = "energy-client-id";
    private const string HttpContextItemKey = "energy-client-id";

    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>HTTP bağlam erişimcisi ile servisi başlatır.</summary>
    public BrowserClientIdService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>Tarayıcı istemci kimliğini döndürür; yoksa oluşturup çereze kaydeder.</summary>
    public string GetOrCreate()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext is null)
        {
            return $"WEB-SERVER-{Environment.MachineName}";
        }

        if (httpContext.Items.TryGetValue(HttpContextItemKey, out var itemValue)
            && itemValue is string cached
            && !string.IsNullOrWhiteSpace(cached))
        {
            return cached;
        }

        if (httpContext.Request.Cookies.TryGetValue(CookieName, out var cookieValue)
            && !string.IsNullOrWhiteSpace(cookieValue))
        {
            httpContext.Items[HttpContextItemKey] = cookieValue;
            return cookieValue;
        }

        var clientId = $"WEB-{Guid.NewGuid():N}";
        httpContext.Items[HttpContextItemKey] = clientId;

        httpContext.Response.Cookies.Append(
            CookieName,
            clientId,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                IsEssential = true,
                Expires = DateTimeOffset.UtcNow.AddYears(1)
            });

        return clientId;
    }
}

