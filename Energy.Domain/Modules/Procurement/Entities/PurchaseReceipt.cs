using Energy.Domain.Common;

namespace Energy.Domain.Modules.Procurement;

/// <summary>
/// Mal kabul başlıkları
/// </summary>
public class PurchaseReceipt : AuditableEntity
{
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
