using Energy.Domain.Common;

namespace Energy.Domain.Operations;

/// <summary>İş emri görev ataması (kullanıcı veya personel).</summary>
public class WorkOrderAssignment : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? UserId { get; set; }
    public string? AssignmentRole { get; set; }
}
