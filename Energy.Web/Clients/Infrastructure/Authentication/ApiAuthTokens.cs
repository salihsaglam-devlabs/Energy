namespace Energy.Web.Clients.Infrastructure.Authentication;

/// <summary>
/// Names of the auth ticket tokens stored in the user's cookie principal at
/// sign-in. The bearer token attached to outbound API calls is read from these
/// values via <see cref="IUserApiTokenProvider"/>.
/// </summary>
public static class ApiAuthTokens
{
    public const string AccessToken = "access_token";
    public const string ExpiresAt = "expires_at";
}

