using Energy.Domain.Common;

namespace Energy.Domain.Organization;

/// <summary>Yetkinlik tanımı.</summary>
public class EmployeeSkill : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
