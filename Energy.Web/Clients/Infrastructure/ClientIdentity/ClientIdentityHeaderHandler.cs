using System.Globalization;

namespace Energy.Web.Clients.Infrastructure.ClientIdentity;

/// <summary>
/// Adds <c>X-Client-Id</c>, <c>X-Client-Machine-Name</c> and forwards the
/// browser's <c>User-Agent</c> to every outbound API request, so the API can
/// audit calls per browser session and per Web server instance.
/// </summary>
public sealed class ClientIdentityHeaderHandler : DelegatingHandler
{
    private const string ClientIdHeader = "X-Client-Id";
    private const string ClientMachineNameHeader = "X-Client-Machine-Name";
    private const string UserAgentHeader = "User-Agent";
    private const string AcceptLanguageHeader = "Accept-Language";

    private readonly BrowserClientIdService _clientIdService;
    private readonly IHttpContextAccessor _httpContextAccessor;

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

