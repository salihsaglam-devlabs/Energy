namespace Energy.Web.Clients.Infrastructure.Authentication;

/// <summary>
/// O an oturum açmış kullanıcının bearer erişim jetonunu döndürür (çerez kimlik doğrulama
/// biletinden okunur). Anonim istekler için <c>null</c> döndürür.
/// </summary>
public interface IUserApiTokenProvider
{
    /// <summary>O an oturum açmış kullanıcının erişim jetonunu döndürür (anonimse null).</summary>
    Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Döndürülen kapsamın (scope) ömrü boyunca çerez tabanlı aramayı geçersiz kılan
    /// açık bir erişim jetonu ayarlar. Kimlik doğrulama çerezi henüz yazılmamışken ancak
    /// alt API çağrılarının (örn. rol/yetki aramaları) yine de bir bearer jetona ihtiyaç
    /// duyduğu giriş sırasında kullanışlıdır.
    /// </summary>
    IDisposable UseAccessToken(string accessToken);
}

