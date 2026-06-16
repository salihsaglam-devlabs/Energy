using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Workflow;

/// <summary>Onay adımı tanımı.</summary>
public class ApprovalStepDefinition : AuditableEntity
{
    public Guid ApprovalDefinitionVersionId { get; set; }
    public int StepNo { get; set; }
    public string Name { get; set; } = string.Empty;
    public ApprovalMode ApprovalMode { get; set; } = ApprovalMode.Sequential;
    public int? RequiredApprovalCount { get; set; }
    public bool IsRequired { get; set; } = true;
}
