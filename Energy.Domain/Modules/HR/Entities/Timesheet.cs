using Energy.Domain.Common;

namespace Energy.Domain.Modules.HR;

/// <summary>
/// Puantaj başlıkları
/// </summary>
public class Timesheet : AuditableEntity
{
    /// <summary>TimesheetNo</summary>
    public string TimesheetNo { get; set; } = string.Empty;

    /// <summary>PeriodStart</summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>PeriodEnd</summary>
    public DateTime PeriodEnd { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
