namespace Energy.Shared.Models.V1.IAM.UserRole.Requests;

/// <summary>UserRole oluşturma isteği.</summary>
public class CreateUserRoleRequest
{
    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>Roles referansı</summary>
    public Guid RoleId { get; set; }
}
