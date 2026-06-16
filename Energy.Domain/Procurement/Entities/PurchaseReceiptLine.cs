using Energy.Domain.Common;

namespace Energy.Domain.Procurement;

/// <summary>Mal kabul satırı.</summary>
public class PurchaseReceiptLine : AuditableEntity
{
    public Guid PurchaseReceiptId { get; set; }
    public Guid? PurchaseOrderLineId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
