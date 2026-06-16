using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>
/// Departman hiyerarşisi
/// </summary>
public class Department : AuditableEntity
{
    /// <summary>Şirket</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Üst departman</summary>
    public Guid? ParentDepartmentId { get; set; }

    /// <summary>Departman kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Departman adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>ManagerUserId</summary>
    public Guid? ManagerUserId { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
