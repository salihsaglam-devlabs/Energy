using System.Globalization;

namespace Energy.Web.Clients.Infrastructure.ClientIdentity;

/// <summary>
/// Her giden API isteğine <c>X-Client-Id</c>, <c>X-Client-Machine-Name</c> başlıklarını
/// ekler ve tarayıcının <c>User-Agent</c> bilgisini iletir; böylece API, çağrıları
/// tarayıcı oturumu ve Web sunucusu örneği başına denetleyebilir.
/// </summary>
public sealed class ClientIdentityHeaderHandler : DelegatingHandler
{
    private const string ClientIdHeader = "X-Client-Id";
    private const string ClientMachineNameHeader = "X-Client-Machine-Name";
    private const string UserAgentHeader = "User-Agent";
    private const string AcceptLanguageHeader = "Accept-Language";

    private readonly BrowserClientIdService _clientIdService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>İstemci kimliği servisi ve HTTP bağlam erişimcisi ile işleyiciyi başlatır.</summary>
    public ClientIdentityHeaderHandler(
        BrowserClientIdService clientIdService,
        IHttpContextAccessor httpContextAccessor)
    {
        _clientIdService = clientIdService;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        SetHeader(request, ClientIdHeader, _clientIdService.GetOrCreate());
        SetHeader(request, ClientMachineNameHeader, Environment.MachineName);
        SetHeader(request, AcceptLanguageHeader, CultureInfo.CurrentUICulture.Name);

        var userAgent = _httpContextAccessor.HttpContext?
            .Request.Headers.UserAgent.FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(userAgent))
        {
            request.Headers.Remove(UserAgentHeader);
            request.Headers.TryAddWithoutValidation(UserAgentHeader, userAgent);
        }

        return base.SendAsync(request, cancellationToken);
    }

    private static void SetHeader(HttpRequestMessage request, string name, string value)
    {
        request.Headers.Remove(name);
        request.Headers.Add(name, value);
    }
}

