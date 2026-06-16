using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Kullanıcı bazlı permission istisnaları
/// </summary>
public class UserPermission : AuditableEntity
{
    /// <summary>Users referansı</summary>
    public Guid UserId { get; set; }

    /// <summary>Permissions referansı</summary>
    public string PermissionCode { get; set; } = string.Empty;
}
