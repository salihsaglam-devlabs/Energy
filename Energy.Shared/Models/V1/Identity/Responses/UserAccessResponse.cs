namespace Energy.Shared.Models.V1.Identity.Responses;

/// <summary>
/// Özel erişim yönetimi ekranı için tek bir kullanıcının erişiminin tam görünümü:
/// atanmış roller, bu roller aracılığıyla miras alınan yetkiler (salt okunur) ve
/// üzerine eklenen doğrudan (kullanıcıya özel) tanımlar.
/// </summary>
public sealed class UserAccessResponse
{
    /// <summary>Kullanıcının kimliği.</summary>
    public Guid UserId { get; init; }

    /// <summary>Kullanıcı adı.</summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>Kullanıcının ad soyadı.</summary>
    public string FullName { get; init; } = string.Empty;

    /// <summary>Kullanıcının etkin olup olmadığı.</summary>
    public bool IsActive { get; init; }

    /// <summary>Kullanıcıya şu anda atanmış roller.</summary>
    public IReadOnlyList<Guid> RoleIds { get; init; } = Array.Empty<Guid>();

    /// <summary>Kullanıcının rolleri aracılığıyla sahip olduğu yetki kodları (miras, salt okunur).</summary>
    public IReadOnlyList<string> RolePermissionCodes { get; init; } = Array.Empty<string>();

    /// <summary>Rol yetkilerine ek olarak kullanıcıya doğrudan verilen yetki kodları.</summary>
    public IReadOnlyList<string> DirectPermissionCodes { get; init; } = Array.Empty<string>();
}
