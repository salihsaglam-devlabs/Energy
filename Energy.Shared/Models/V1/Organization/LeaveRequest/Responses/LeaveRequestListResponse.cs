using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;

/// <summary>LeaveRequest liste satırı.</summary>
public class LeaveRequestListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>EmployeeId</summary>
    public Guid EmployeeId { get; set; }

    /// <summary>LeaveType</summary>
    public string LeaveType { get; set; } = string.Empty;

    /// <summary>StartDate</summary>
    public DateTime StartDate { get; set; }

    /// <summary>EndDate</summary>
    public DateTime EndDate { get; set; }

    /// <summary>Days</summary>
    public decimal Days { get; set; }

    /// <summary>Reason</summary>
    public string? Reason { get; set; }

    /// <summary>Status</summary>
    public ApprovalRequestStatus Status { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
