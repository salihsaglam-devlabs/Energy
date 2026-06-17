namespace Energy.Shared.Models.V1.IAM.RolePermission.Requests;

/// <summary>RolePermission güncelleme isteği.</summary>
public class UpdateRolePermissionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Roles referansı</summary>
    public Guid RoleId { get; set; }

    /// <summary>Permissions referansı</summary>
    public string PermissionCode { get; set; } = string.Empty;
}
