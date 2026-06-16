using Energy.Domain.Common;

namespace Energy.Domain.Modules.Procurement;

/// <summary>Tedarikçi teklif satırı.</summary>
public class SupplierQuoteLine : AuditableEntity
{
    public Guid SupplierQuoteId { get; set; }
    public Guid? RequestLineId { get; set; }
    public Guid? MaterialId { get; set; }
    public string? Description { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TaxRate { get; set; }
    public decimal DiscountRate { get; set; }
    public int DeliveryDays { get; set; }
}
