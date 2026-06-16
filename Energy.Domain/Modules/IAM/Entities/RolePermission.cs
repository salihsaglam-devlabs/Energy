using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Rol permission bağlantıları
/// </summary>
public class RolePermission : AuditableEntity
{
    /// <summary>Roles referansı</summary>
    public Guid RoleId { get; set; }

    /// <summary>Permissions referansı</summary>
    public string PermissionCode { get; set; } = string.Empty;
}
