using Energy.Domain.Common;

namespace Energy.Domain.Operations;

/// <summary>Planlanan iş emri malzemesi.</summary>
public class WorkOrderMaterialPlan : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal PlannedQuantity { get; set; }
}
