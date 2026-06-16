using Energy.Domain.Common;

namespace Energy.Domain.Modules.Workflow;

/// <summary>Onay akışı tanımı.</summary>
public class ApprovalDefinition : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string RelatedModule { get; set; } = string.Empty;
    public string RelatedEntityType { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
