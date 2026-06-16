using Energy.Domain.Common;

namespace Energy.Domain.Modules.Organization;

/// <summary>
/// Personel yetkinlik bağlantıları
/// </summary>
public class EmployeeSkillAssignment : AuditableEntity
{
    /// <summary>EmployeeId</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>EmployeeSkillId</summary>
    public Guid EmployeeSkillId { get; set; }

    /// <summary>Level</summary>
    public int Level { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
