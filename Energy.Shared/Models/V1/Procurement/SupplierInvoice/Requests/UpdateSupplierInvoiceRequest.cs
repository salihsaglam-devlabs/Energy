using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Procurement.SupplierInvoice.Requests;

/// <summary>SupplierInvoice güncelleme isteği.</summary>
public class UpdateSupplierInvoiceRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
    public DocumentStatus Status { get; set; }
}
