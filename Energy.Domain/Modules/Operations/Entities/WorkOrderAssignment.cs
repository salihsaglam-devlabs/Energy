using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>
/// İş emri görev atamaları
/// </summary>
public class WorkOrderAssignment : AuditableEntity
{
    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Employees referansı</summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>Users referansı</summary>
    public Guid? UserId { get; set; }

    /// <summary>AssignmentRole</summary>
    public string? AssignmentRole { get; set; }
}
