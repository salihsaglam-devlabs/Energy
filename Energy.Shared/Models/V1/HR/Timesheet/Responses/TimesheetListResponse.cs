using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.HR.Timesheet.Responses;

/// <summary>Timesheet liste satırı.</summary>
public class TimesheetListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
