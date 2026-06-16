using Energy.Domain.Common;

namespace Energy.Domain.Procurement;

/// <summary>Tedarikçi fatura satırı.</summary>
public class SupplierInvoiceLine : AuditableEntity
{
    public Guid SupplierInvoiceId { get; set; }
    public Guid? MaterialId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TaxRate { get; set; }
}
