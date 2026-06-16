namespace Energy.Shared.Models.V1.Procurement.PurchaseReceiptLine.Requests;

/// <summary>PurchaseReceiptLine oluşturma isteği.</summary>
public class CreatePurchaseReceiptLineRequest
{
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
}
