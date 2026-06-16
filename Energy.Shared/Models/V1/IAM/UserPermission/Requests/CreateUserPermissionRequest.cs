namespace Energy.Shared.Models.V1.IAM.UserPermission.Requests;

/// <summary>UserPermission oluşturma isteği.</summary>
public class CreateUserPermissionRequest
{
    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>Permissions referansı</summary>
    public string PermissionCode { get; set; } = string.Empty;
}
