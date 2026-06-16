using Energy.Domain.Common;

namespace Energy.Domain.Finance;

/// <summary>Maliyet merkezi.</summary>
public class CostCenter : AuditableEntity
{
    public Guid? ParentCostCenterId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
