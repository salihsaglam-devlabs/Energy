using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Workflow;

/// <summary>Talebe bağlı adım örneği.</summary>
public class ApprovalRequestStep : AuditableEntity
{
    public Guid ApprovalRequestId { get; set; }
    public Guid ApprovalStepDefinitionId { get; set; }
    public int StepNo { get; set; }
    public ApprovalMode ApprovalMode { get; set; } = ApprovalMode.Sequential;
    public int? RequiredApprovalCount { get; set; }
    public ApprovalStepStatus Status { get; set; } = ApprovalStepStatus.Waiting;
}
