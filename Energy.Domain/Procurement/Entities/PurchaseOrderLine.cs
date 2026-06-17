using Energy.Domain.Common;

namespace Energy.Domain.Procurement;

/// <summary>Satın alma sipariş satırı.</summary>
public class PurchaseOrderLine : AuditableEntity
{
    public Guid PurchaseOrderId { get; set; }
    public Guid? RequestLineId { get; set; }
    public Guid? MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Guid CurrencyId { get; set; }
    public decimal ReceivedQuantity { get; set; }
}
