namespace Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;

/// <summary>WorkOrderMaterialUsage güncelleme isteği.</summary>
public class UpdateWorkOrderMaterialUsageRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>StockDocumentLines referansı</summary>
    public Guid? StockDocumentLineId { get; set; }

    /// <summary>MaterialId</summary>
    public Guid MaterialId { get; set; }

    /// <summary>UsedQuantity</summary>
    public decimal UsedQuantity { get; set; }
}
