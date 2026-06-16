using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>Adımın gerçek onaycıları (snapshot).</summary>
public class ApprovalRequestApprover : AuditableEntity
{
    public Guid ApprovalRequestStepId { get; set; }
    public Guid UserId { get; set; }
    public ApprovalApproverStatus Status { get; set; } = ApprovalApproverStatus.Waiting;
    public DateTime? ActionAt { get; set; }
    /// <summary>Devralan kullanıcı (delegation çözümlendiyse).</summary>
    public Guid? DelegatedFromUserId { get; set; }
}
