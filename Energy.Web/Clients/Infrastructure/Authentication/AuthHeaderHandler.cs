using System.Net;
using System.Net.Http.Headers;
using Energy.Web.Common.Exceptions;

namespace Energy.Web.Clients.Infrastructure.Authentication;

/// <summary>
/// Outgoing HttpClient handler that attaches the signed-in user's JWT to every
/// authenticated API call and converts auth-related status codes into
/// dedicated exceptions handled by <c>ApiExceptionFilter</c>:
/// 401 Unauthorized maps to <see cref="ApiUnauthorizedException"/>,
/// 403 Forbidden maps to <see cref="ApiForbiddenException"/>.
/// </summary>
public sealed class AuthHeaderHandler : DelegatingHandler
{
    private readonly IUserApiTokenProvider _tokenProvider;

    public AuthHeaderHandler(IUserApiTokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetAccessTokenAsync(cancellationToken);

        if (string.IsNullOrEmpty(token))
        {
            // No signed-in user: surface as Unauthorized so the filter
            // redirects to the login page rather than returning a confusing
            // API error to the browser.
            throw new ApiUnauthorizedException();
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                response.Dispose();
                throw new ApiUnauthorizedException();
            case HttpStatusCode.Forbidden:
                response.Dispose();
                throw new ApiForbiddenException();
            default:
                return response;
        }
    }
}

