using Energy.Domain.Common;

namespace Energy.Domain.Organization;

/// <summary>Personel ↔ yetkinlik N:N bağlantısı.</summary>
public class EmployeeSkillAssignment : AuditableEntity
{
    public Guid EmployeeId { get; set; }
    public Guid EmployeeSkillId { get; set; }
    public int Level { get; set; }
    public string? Note { get; set; }
}
