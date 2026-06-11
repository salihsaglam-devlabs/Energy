using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Energy.Localization;

namespace Energy.Web.Clients.Infrastructure.Authentication;

public sealed class UserApiTokenProvider : IUserApiTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private static readonly AsyncLocal<string?> OverrideToken = new();

    public UserApiTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(OverrideToken.Value))
        {
            return OverrideToken.Value;
        }

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
        {
            return null;
        }

        if (httpContext.User.Identity?.IsAuthenticated != true)
        {
            return null;
        }

        var token = await httpContext.GetTokenAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            ApiAuthTokens.AccessToken);

        return string.IsNullOrEmpty(token) ? null : token;
    }

    public IDisposable UseAccessToken(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new ArgumentException(
                LocalizationText.Get(
                    LocalizationKeys.Messages.AccessTokenRequired,
                    "Access token must not be empty."),
                nameof(accessToken));
        }

        var previous = OverrideToken.Value;
        OverrideToken.Value = accessToken;
        return new TokenScope(previous);
    }

    private sealed class TokenScope : IDisposable
    {
        private readonly string? _previous;
        private bool _disposed;

        public TokenScope(string? previous)
        {
            _previous = previous;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            OverrideToken.Value = _previous;
            _disposed = true;
        }
    }
}

