using Energy.Domain.Common;

namespace Energy.Domain.Modules.Procurement;

/// <summary>
/// Satın alma sipariş satırları
/// </summary>
public class PurchaseOrderLine : AuditableEntity
{
    /// <summary>Sipariş</summary>
    public Guid PurchaseOrderId { get; set; }

    /// <summary>Talep satırı</summary>
    public Guid? RequestLineId { get; set; }

    /// <summary>Malzeme</summary>
    public Guid? MaterialId { get; set; }

    /// <summary>Miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Fiyat</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Para birimi</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>ReceivedQuantity</summary>
    public decimal ReceivedQuantity { get; set; }
}
