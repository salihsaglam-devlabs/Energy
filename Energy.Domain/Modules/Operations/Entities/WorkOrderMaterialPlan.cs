using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>
/// Planlanan iş emri malzemeleri
/// </summary>
public class WorkOrderMaterialPlan : AuditableEntity
{
    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>PlannedQuantity</summary>
    public decimal PlannedQuantity { get; set; }
}
