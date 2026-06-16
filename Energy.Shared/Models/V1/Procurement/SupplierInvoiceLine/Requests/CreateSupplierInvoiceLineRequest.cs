namespace Energy.Shared.Models.V1.Procurement.SupplierInvoiceLine.Requests;

/// <summary>SupplierInvoiceLine oluşturma isteği.</summary>
public class CreateSupplierInvoiceLineRequest
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
