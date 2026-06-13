namespace Energy.Shared.Models.V1.Identity.Responses;

/// <summary>Bir rolün ayrıntılı görünümü: yetki kodları ve rolü taşıyan kullanıcılar dahil.</summary>
public sealed class RoleDetailResponse
{
    /// <summary>Rolün kimliği.</summary>
    public Guid Id { get; init; }

    /// <summary>Rolün adı.</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Rolün açıklaması.</summary>
    public string? Description { get; init; }

    /// <summary>Rolün sistem rolü olup olmadığı (sistem rolleri silinemez).</summary>
    public bool IsSystem { get; init; }

    /// <summary>Role atanmış yetki kodları.</summary>
    public IReadOnlyCollection<string> PermissionCodes { get; init; } = Array.Empty<string>();

    /// <summary>Bu role sahip kullanıcılar.</summary>
    public IReadOnlyCollection<UserSummaryResponse> Users { get; init; } = Array.Empty<UserSummaryResponse>();
}
