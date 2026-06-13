using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Energy.Web.Common.Exceptions;

namespace Energy.Web.Clients.Infrastructure.Authentication;

/// <summary>
/// Oturum açmış kullanıcının JWT'sini kimliği doğrulanmış her API çağrısına ekleyen
/// ve kimlik doğrulamayla ilgili durum kodlarını <c>ApiExceptionFilter</c> tarafından
/// işlenen özel istisnalara dönüştüren giden HttpClient işleyicisi.
/// </summary>
public sealed class AuthHeaderHandler : DelegatingHandler
{
    private readonly IUserApiTokenProvider _tokenProvider;
    private readonly ILogger<AuthHeaderHandler> _logger;

    /// <summary>Token sağlayıcısını ve günlükleyiciyi enjekte eder.</summary>
    public AuthHeaderHandler(IUserApiTokenProvider tokenProvider, ILogger<AuthHeaderHandler> logger)
    {
        _tokenProvider = tokenProvider;
        _logger = logger;
    }

    /// <summary>İsteğe yetkilendirme başlığı ekler ve auth durum kodlarını istisnaya dönüştürür.</summary>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetAccessTokenAsync(cancellationToken);

        if (string.IsNullOrEmpty(token))
        {
            _logger.LogWarning("[AuthHeader] No access token for {Method} {Uri}; redirecting to login.",
                request.Method, request.RequestUri);
            throw new ApiUnauthorizedException("No access token for current user.");
        }

        // Tanılama: JWT yükünü çöz (İMZA DOĞRULAMASI YOK — yalnızca API'ye tam olarak
        // ne gönderdiğimizi Web konsolunda görebilmek için).
        LogTokenSummary(request, token);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
            {
                var body = await SafeReadBodyAsync(response, cancellationToken);
                var wwwAuth = response.Headers.WwwAuthenticate.ToString();
                response.Dispose();
                _logger.LogWarning(
                    "[AuthHeader] API 401 for {Method} {Uri}. WWW-Authenticate={WwwAuth} Body={Body}",
                    request.Method, request.RequestUri, wwwAuth, body);
                throw new ApiUnauthorizedException(
                    $"API 401 for {request.Method} {request.RequestUri}. WWW-Authenticate={wwwAuth}. Body={body}");
            }
            case HttpStatusCode.Forbidden:
            {
                var body = await SafeReadBodyAsync(response, cancellationToken);
                response.Dispose();
                _logger.LogWarning("[AuthHeader] API 403 for {Method} {Uri}. Body={Body}",
                    request.Method, request.RequestUri, body);
                throw new ApiForbiddenException(
                    $"API 403 for {request.Method} {request.RequestUri}. Body={body}");
            }
            default:
                return response;
        }
    }

    /// <summary>Tanılama amacıyla JWT yükünün bir özetini günlüğe yazar (imza doğrulamaz).</summary>
    private void LogTokenSummary(HttpRequestMessage request, string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length < 2)
            {
                _logger.LogWarning("[AuthHeader] Token is not a JWT (parts={Parts}). len={Len}",
                    parts.Length, token.Length);
                return;
            }

            var payloadJson = DecodeBase64UrlToUtf8(parts[1]);
            using var doc = JsonDocument.Parse(payloadJson);
            var root = doc.RootElement;
            string? Get(string name) => root.TryGetProperty(name, out var v) ? v.ToString() : null;
            long? GetLong(string name) =>
                root.TryGetProperty(name, out var v) && v.TryGetInt64(out var l) ? l : null;

            var sub = Get("sub");
            var sst = Get("sst");
            var iss = Get("iss");
            var aud = Get("aud");
            var nbf = GetLong("nbf");
            var exp = GetLong("exp");
            var nowUnix = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            _logger.LogInformation(
                "[AuthHeader] Sending {Method} {Uri} | jwt.len={Len} sub={Sub} sst={Sst} iss={Iss} aud={Aud} nbf={Nbf} exp={Exp} now={Now} validNow={Valid}",
                request.Method, request.RequestUri, token.Length, sub, sst, iss, aud,
                nbf, exp, nowUnix,
                nbf is null || exp is null ? "?" : (nowUnix >= nbf - 30 && nowUnix <= exp + 30).ToString());
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[AuthHeader] Failed to decode JWT for logging.");
        }
    }

    /// <summary>Base64Url kodlu bir dizeyi çözerek UTF-8 metne dönüştürür.</summary>
    private static string DecodeBase64UrlToUtf8(string base64Url)
    {
        var s = base64Url.Replace('-', '+').Replace('_', '/');
        switch (s.Length % 4)
        {
            case 2: s += "=="; break;
            case 3: s += "="; break;
        }
        return Encoding.UTF8.GetString(Convert.FromBase64String(s));
    }

    /// <summary>Yanıt gövdesini güvenli şekilde (hata fırlatmadan) metin olarak okur.</summary>
    private static async Task<string> SafeReadBodyAsync(HttpResponseMessage response, CancellationToken ct)
    {
        try
        {
            var raw = await response.Content.ReadAsStringAsync(ct);
            return string.IsNullOrWhiteSpace(raw) ? "<empty>" : raw;
        }
        catch (Exception ex) { return $"<failed: {ex.Message}>"; }
    }
}
