using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Organization;

/// <summary>İzin talebi. Workflow onayına bağlanabilir.</summary>
public class LeaveRequest : AuditableEntity
{
    public Guid EmployeeId { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Days { get; set; }
    public string? Reason { get; set; }
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public Guid? ApprovalRequestId { get; set; }
}
