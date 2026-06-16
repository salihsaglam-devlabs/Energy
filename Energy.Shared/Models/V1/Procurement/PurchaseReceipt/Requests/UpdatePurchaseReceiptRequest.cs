namespace Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Requests;

/// <summary>PurchaseReceipt güncelleme isteği.</summary>
public class UpdatePurchaseReceiptRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>SupplierId</summary>
    public Guid SupplierId { get; set; }

    /// <summary>PurchaseOrderId</summary>
    public Guid? PurchaseOrderId { get; set; }

    /// <summary>WarehouseId</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>StockDocumentId</summary>
    public Guid? StockDocumentId { get; set; }

    /// <summary>ReceiptNo</summary>
    public string ReceiptNo { get; set; } = string.Empty;

    /// <summary>ReceiptDate</summary>
    public DateTime ReceiptDate { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;
}
