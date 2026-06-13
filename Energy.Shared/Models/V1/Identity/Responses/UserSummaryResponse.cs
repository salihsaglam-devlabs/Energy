namespace Energy.Shared.Models.V1.Identity.Responses;

/// <summary>Liste görünümleri için bir kullanıcının özet bilgisi.</summary>
public sealed class UserSummaryResponse
{
    /// <summary>Kullanıcının kimliği.</summary>
    public Guid Id { get; init; }

    /// <summary>Kullanıcı adı.</summary>
    public string UserName { get; init; } = string.Empty;

    /// <summary>E-posta adresi.</summary>
    public string Email { get; init; } = string.Empty;

    /// <summary>Kullanıcının ad soyadı.</summary>
    public string FullName { get; init; } = string.Empty;

    /// <summary>Kullanıcının etkin olup olmadığı.</summary>
    public bool IsActive { get; init; }

    /// <summary>Son giriş zamanı.</summary>
    public DateTime? LastLoginAt { get; init; }

    /// <summary>Kullanıcının sahip olduğu rol adları.</summary>
    public IReadOnlyCollection<string> RoleNames { get; init; } = Array.Empty<string>();
}
