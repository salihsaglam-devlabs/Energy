namespace Energy.Shared.Models.V1.IAM.RolePermission.Responses;

/// <summary>RolePermission liste satırı.</summary>
public class RolePermissionListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Roles referansı</summary>
    public Guid RoleId { get; set; }

    /// <summary>Permissions referansı</summary>
    public string PermissionCode { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
