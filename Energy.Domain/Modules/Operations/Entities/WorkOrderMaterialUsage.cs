using Energy.Domain.Common;

namespace Energy.Domain.Modules.Operations;

/// <summary>
/// Gerçekleşen iş emri malzeme kullanımları
/// </summary>
public class WorkOrderMaterialUsage : AuditableEntity
{
    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>StockDocumentLines referansı</summary>
    public Guid? StockDocumentLineId { get; set; }

    /// <summary>MaterialId</summary>
    public Guid MaterialId { get; set; }

    /// <summary>UsedQuantity</summary>
    public decimal UsedQuantity { get; set; }
}
