namespace Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Responses;

/// <summary>PurchaseReceiptLine liste satırı.</summary>
public class PurchaseReceiptLineListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>PurchaseReceipts referansı</summary>
    public Guid PurchaseReceiptId { get; set; }

    /// <summary>PurchaseOrderLines referansı</summary>
    public Guid? PurchaseOrderLineId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
