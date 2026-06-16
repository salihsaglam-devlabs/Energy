using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.HR;

/// <summary>Puantaj başlığı.</summary>
public class Timesheet : AuditableEntity
{
    public string TimesheetNo { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}
