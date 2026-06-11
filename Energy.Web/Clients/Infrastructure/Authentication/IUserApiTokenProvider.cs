namespace Energy.Web.Clients.Infrastructure.Authentication;

/// <summary>
/// Returns the bearer access token of the currently signed-in user (read from
/// the cookie auth ticket). Returns <c>null</c> for anonymous requests.
/// </summary>
public interface IUserApiTokenProvider
{
    Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets an explicit access token that overrides the cookie-based lookup
    /// for the lifetime of the returned scope. Useful during sign-in, when
    /// the auth cookie has not been written yet but downstream API calls
    /// (e.g. role/permission lookups) still need a bearer token.
    /// </summary>
    IDisposable UseAccessToken(string accessToken);
}

