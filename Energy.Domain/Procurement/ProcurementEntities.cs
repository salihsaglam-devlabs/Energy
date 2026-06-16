using Energy.Domain.Common;

namespace Energy.Domain.Procurement;

/// <summary>Tedarikçi teklif başlığı.</summary>
public class SupplierQuote : AuditableEntity
{
    public Guid SupplierId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid CurrencyId { get; set; }
    public string QuoteNo { get; set; } = string.Empty;
    public DateTime QuoteDate { get; set; }
    public string? PaymentTerm { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
    public bool IsSelected { get; set; }
}

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

/// <summary>Satın alma sipariş başlığı.</summary>
public class PurchaseOrder : AuditableEntity
{
    public Guid SupplierId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid CurrencyId { get; set; }
    public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Draft;
    public string OrderNo { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public Guid? ApprovalRequestId { get; set; }
}

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

/// <summary>Mal kabul başlığı.</summary>
public class PurchaseReceipt : AuditableEntity
{
    public Guid SupplierId { get; set; }
    public Guid? PurchaseOrderId { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid? StockDocumentId { get; set; }
    public string ReceiptNo { get; set; } = string.Empty;
    public DateTime ReceiptDate { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;
}

/// <summary>Mal kabul satırı.</summary>
public class PurchaseReceiptLine : AuditableEntity
{
    public Guid PurchaseReceiptId { get; set; }
    public Guid? PurchaseOrderLineId { get; set; }
    public Guid MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

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

