namespace Energy.Shared.Models.V1.IAM.UserRole.Responses;

/// <summary>UserRole liste satırı.</summary>
public class UserRoleListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>Roles referansı</summary>
    public Guid RoleId { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
