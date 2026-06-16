using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Procurement;

/// <summary>Mal kabul başlığı.</summary>
public class PurchaseReceipt : AuditableEntity
{
    public Guid SupplierId { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid? StockDocumentId { get; set; }
    public string ReceiptNo { get; set; } = string.Empty;
    public DateTime ReceiptDate { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}
