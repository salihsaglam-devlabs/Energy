using Energy.Domain.Common;

namespace Energy.Domain.Operations;

/// <summary>Gerçekleşen iş emri malzeme kullanımı.</summary>
public class WorkOrderMaterialUsage : AuditableEntity
{
    public Guid WorkOrderId { get; set; }
    public Guid? StockDocumentLineId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal UsedQuantity { get; set; }
}
