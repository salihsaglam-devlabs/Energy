namespace Energy.Shared.Models.V1.Organization.LeaveRequest.Requests;

/// <summary>LeaveRequest oluşturma isteği.</summary>
public class CreateLeaveRequestRequest
{
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
    public string Status { get; set; } = string.Empty;

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
