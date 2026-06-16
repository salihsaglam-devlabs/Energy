using Energy.Domain.Common;

namespace Energy.Domain.Modules.Procurement;

/// <summary>
/// Tedarikçi fatura satırları
/// </summary>
public class SupplierInvoiceLine : AuditableEntity
{
    /// <summary>SupplierInvoiceId</summary>
    public Guid SupplierInvoiceId { get; set; }

    /// <summary>MaterialId</summary>
    public Guid? MaterialId { get; set; }

    /// <summary>Description</summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>TaxRate</summary>
    public decimal TaxRate { get; set; }
}
