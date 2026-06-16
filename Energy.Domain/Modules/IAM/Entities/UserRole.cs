using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Kullanıcı rol bağlantıları
/// </summary>
public class UserRole : AuditableEntity
{
    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>Roles referansı</summary>
    public Guid RoleId { get; set; }
}
