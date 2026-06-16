using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Roller
/// </summary>
public class Role : AuditableEntity
{
    /// <summary>Rol adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Açıklama</summary>
    public string? Description { get; set; }

    /// <summary>Sistem rolü</summary>
    public bool IsSystem { get; set; }
}
