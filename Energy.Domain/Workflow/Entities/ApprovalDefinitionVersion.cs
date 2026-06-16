using Energy.Domain.Common;

namespace Energy.Domain.Workflow;

/// <summary>Onay akışı versiyonu.</summary>
public class ApprovalDefinitionVersion : AuditableEntity
{
    public Guid ApprovalDefinitionId { get; set; }
    public int VersionNo { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public bool IsActive { get; set; } = true;
}
