namespace Energy.Shared.Models.V1.Identity.Responses;

/// <summary>Başarılı bir girişin sonucu: erişim jetonu ve kullanıcı kimlik/yetki bilgileri.</summary>
public sealed class AuthTokenResponse
{
    /// <summary>API çağrılarında kullanılacak erişim jetonu (JWT).</summary>
    public string AccessToken { get; init; } = string.Empty;

    /// <summary>Jetonun geçerlilik bitiş zamanı.</summary>
    public DateTime ExpiresAt { get; init; }

    /// <summary>Kullanıcının kimliği.</summary>
    public Guid UserId { get; init; }

    /// <summary>Kullanıcı adı.</summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>Arayüzde gösterilecek görünen ad.</summary>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>Kullanıcının ait olduğu rol adları (gösterim / arayüz gruplaması için).</summary>
    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Etkin yetki kodları (Kullanıcı → Rol → Yetki). Web katmanında arayüz
    /// yetkilendirmesini (menü, sayfa ve aksiyon kısıtlaması) yönlendirir.
    /// </summary>
    public IReadOnlyList<string> Permissions { get; init; } = Array.Empty<string>();
}
