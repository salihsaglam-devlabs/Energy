using Energy.Domain.Common;

namespace Energy.Domain.Modules.Procurement;

/// <summary>
/// Tedarikçi teklif satırları
/// </summary>
public class SupplierQuoteLine : AuditableEntity
{
    /// <summary>SupplierQuotes referansı</summary>
    public Guid SupplierQuoteId { get; set; }

    /// <summary>RequestLines referansı</summary>
    public Guid? RequestLineId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid? MaterialId { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>TaxRate</summary>
    public decimal TaxRate { get; set; }

    /// <summary>DiscountRate</summary>
    public decimal DiscountRate { get; set; }

    /// <summary>DeliveryDays</summary>
    public int DeliveryDays { get; set; }
}
