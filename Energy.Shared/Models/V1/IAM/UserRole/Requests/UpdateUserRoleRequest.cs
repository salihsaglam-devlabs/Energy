namespace Energy.Shared.Models.V1.IAM.UserRole.Requests;

/// <summary>UserRole güncelleme isteği.</summary>
public class UpdateUserRoleRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>Roles referansı</summary>
    public Guid RoleId { get; set; }
}
