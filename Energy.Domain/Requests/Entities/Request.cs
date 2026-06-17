using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Requests;

/// <summary>Genel talep başlığı.</summary>
public class Request : AuditableEntity
{
    public Guid RequestTypeId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public RequestStatus Status { get; set; } = RequestStatus.Draft;
    public string RequestNo { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string? Description { get; set; }
    public Guid? ApprovalRequestId { get; set; }
}
