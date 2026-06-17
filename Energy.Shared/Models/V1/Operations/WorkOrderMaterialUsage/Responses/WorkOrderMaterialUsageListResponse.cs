namespace Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;

/// <summary>WorkOrderMaterialUsage liste satırı.</summary>
public class WorkOrderMaterialUsageListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>WorkOrders referansı</summary>
    public Guid WorkOrderId { get; set; }

    /// <summary>StockDocumentLines referansı</summary>
    public Guid? StockDocumentLineId { get; set; }

    /// <summary>MaterialId</summary>
    public Guid MaterialId { get; set; }

    /// <summary>UsedQuantity</summary>
    public decimal UsedQuantity { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
