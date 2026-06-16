using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>Çalışan onay talebi.</summary>
public class ApprovalRequest : AuditableEntity
{
    public Guid ApprovalDefinitionVersionId { get; set; }
    public string RelatedModule { get; set; } = string.Empty;
    public string RelatedEntityType { get; set; } = string.Empty;
    public Guid RelatedEntityId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public ApprovalRequestStatus Status { get; set; } = ApprovalRequestStatus.Draft;
    public int CurrentStepNo { get; set; }
}
