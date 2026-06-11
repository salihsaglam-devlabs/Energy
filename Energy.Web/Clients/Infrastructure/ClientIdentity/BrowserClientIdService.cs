namespace Energy.Web.Clients.Infrastructure.ClientIdentity;

/// <summary>
/// Issues and persists a per-browser correlation id via an HTTP cookie so the
/// API can attribute requests to a specific client (separately from the
/// authenticated user). Falls back to a server-machine prefixed id when no
/// HTTP context is available (e.g. background tasks).
/// </summary>
public sealed class BrowserClientIdService
{
    private const string CookieName = "energy-client-id";
    private const string HttpContextItemKey = "energy-client-id";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public BrowserClientIdService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

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

