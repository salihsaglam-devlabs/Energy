using Energy.Domain.Common;

namespace Energy.Domain.Modules.Procurement;

/// <summary>
/// Tedarikçi faturaları
/// </summary>
public class SupplierInvoice : AuditableEntity
{
    /// <summary>SupplierId</summary>
    public Guid SupplierId { get; set; }

    /// <summary>PurchaseOrderId</summary>
    public Guid? PurchaseOrderId { get; set; }

    /// <summary>PurchaseReceiptId</summary>
    public Guid? PurchaseReceiptId { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>InvoiceNo</summary>
    public string InvoiceNo { get; set; } = string.Empty;

    /// <summary>InvoiceDate</summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>TotalAmount</summary>
    public decimal TotalAmount { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;
}
