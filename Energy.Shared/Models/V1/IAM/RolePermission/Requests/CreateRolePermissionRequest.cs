namespace Energy.Shared.Models.V1.IAM.RolePermission.Requests;

/// <summary>RolePermission oluşturma isteği.</summary>
public class CreateRolePermissionRequest
{
    /// <summary>Roles referansı</summary>
    public Guid RoleId { get; set; }

    /// <summary>Permissions referansı</summary>
    public string PermissionCode { get; set; } = string.Empty;
}
