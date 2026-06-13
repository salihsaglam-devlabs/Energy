namespace Energy.Web.Clients.Infrastructure.Authentication;

/// <summary>
/// Girişte kullanıcının çerez kimliğinde (cookie principal) saklanan kimlik doğrulama
/// bileti jetonlarının adları. Giden API çağrılarına eklenen bearer jeton bu değerlerden
/// <see cref="IUserApiTokenProvider"/> aracılığıyla okunur.
/// </summary>
public static class ApiAuthTokens
{
    /// <summary>Erişim jetonu (access token) için bilet anahtarı.</summary>
    public const string AccessToken = "access_token";
    /// <summary>Jetonun geçerlilik bitiş zamanı için bilet anahtarı.</summary>
    public const string ExpiresAt = "expires_at";
}

