using Energy.Shared.Common;
using Energy.Domain.Common;

namespace Energy.Domain.Modules.Procurement;

/// <summary>Tedarikçi faturası.</summary>
public class SupplierInvoice : AuditableEntity
{
    public Guid SupplierId { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public Guid? PurchaseReceiptId { get; set; }
    public Guid CurrencyId { get; set; }
    public string InvoiceNo { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}
