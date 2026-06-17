using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.HR.Timesheet.Requests;

/// <summary>Timesheet oluşturma isteği.</summary>
public class CreateTimesheetRequest
{
    /// <summary>TimesheetNo</summary>
    public string TimesheetNo { get; set; } = string.Empty;

    /// <summary>PeriodStart</summary>
    public DateTime PeriodStart { get; set; }

    /// <summary>PeriodEnd</summary>
    public DateTime PeriodEnd { get; set; }

    /// <summary>Status</summary>
    public ApprovalRequestStatus Status { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
