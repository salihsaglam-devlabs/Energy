namespace Energy.Shared.Models.V1.Identity.Responses;

/// <summary>Bir kullanıcının ayrıntılı görünümü: roller ve etkin yetkiler dahil.</summary>
public sealed class UserDetailResponse
{
    /// <summary>Kullanıcının kimliği.</summary>
    public Guid Id { get; init; }

    /// <summary>Kullanıcı adı.</summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>E-posta adresi.</summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>Adı.</summary>
    public string FirstName { get; init; } = string.Empty;

    /// <summary>Soyadı.</summary>
    public string LastName { get; init; } = string.Empty;

    /// <summary>Kullanıcının etkin olup olmadığı.</summary>
    public bool IsActive { get; init; }

    /// <summary>Kullanıcının oluşturulma zamanı.</summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>Son giriş zamanı.</summary>
    public DateTime? LastLoginAt { get; init; }

    /// <summary>Kullanıcıya atanmış roller.</summary>
    public IReadOnlyCollection<RoleSummaryResponse> Roles { get; init; } = Array.Empty<RoleSummaryResponse>();

    /// <summary>Kullanıcının etkin (toplam) yetki kodları.</summary>
    public IReadOnlyCollection<string> EffectivePermissions { get; init; } = Array.Empty<string>();
}
