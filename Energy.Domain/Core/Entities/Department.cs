using Energy.Domain.Common;

namespace Energy.Domain.Core;

/// <summary>Departman. Şirkete bağlı ve hiyerarşik olabilir.</summary>
public class Department : AuditableEntity
{
    public Guid CompanyId { get; set; }
    public Guid? ParentDepartmentId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Departman yöneticisi (opsiyonel) — DepartmentManager onaycı tipi için kullanılır.</summary>
    public Guid? ManagerUserId { get; set; }
    public bool IsActive { get; set; } = true;
}
