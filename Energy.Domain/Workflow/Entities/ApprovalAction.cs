using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Workflow;

/// <summary>Onay, ret, iade ve iptal hareketleri.</summary>
public class ApprovalAction : AuditableEntity
{
    public Guid ApprovalRequestId { get; set; }
    public Guid? ApprovalRequestStepId { get; set; }
    public Guid UserId { get; set; }
    public ApprovalActionType ActionType { get; set; }
    public DateTime ActionAt { get; set; }
    public string? Note { get; set; }
}
