namespace Energy.Shared.Models.V1.Identity.Responses;

/// <summary>Liste görünümleri için bir rolün özet bilgisi.</summary>
public sealed class RoleSummaryResponse
{
    /// <summary>Rolün kimliği.</summary>
    public Guid Id { get; init; }

    /// <summary>Rolün adı.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Rolün açıklaması.</summary>
    public string? Description { get; init; }

    /// <summary>Rolün sistem rolü olup olmadığı.</summary>
    public bool IsSystem { get; init; }

    /// <summary>Role atanmış yetki sayısı.</summary>
    public int PermissionCount { get; init; }

    /// <summary>Bu role sahip kullanıcı sayısı.</summary>
    public int UserCount { get; init; }
}
