using Energy.Domain.Common;

namespace Energy.Domain.HR;

/// <summary>Puantaj başlığı.</summary>
public class Timesheet : AuditableEntity
{
    public string TimesheetNo { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}

/// <summary>Puantaj satırı.</summary>
public class TimesheetLine : AuditableEntity
{
    public Guid TimesheetId { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? WorkOrderId { get; set; }
    public DateTime WorkDate { get; set; }
    public decimal NormalHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public decimal HourlyCost { get; set; }
}

