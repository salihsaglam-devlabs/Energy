namespace Energy.Shared.Models.V1.IAM.UserPermission.Requests;

/// <summary>UserPermission güncelleme isteği.</summary>
public class UpdateUserPermissionRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>Permissions referansı</summary>
    public string PermissionCode { get; set; } = string.Empty;
}
