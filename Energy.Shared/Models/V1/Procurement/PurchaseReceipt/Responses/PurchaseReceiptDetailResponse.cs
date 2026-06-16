namespace Energy.Shared.Models.V1.Procurement.PurchaseReceipt.Responses;

/// <summary>PurchaseReceipt detay görünümü.</summary>
public class PurchaseReceiptDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>SupplierId</summary>
    public Guid SupplierId { get; set; }

    /// <summary>PurchaseOrderId</summary>
    public Guid? PurchaseOrderId { get; set; }

    /// <summary>WarehouseId</summary>
    public Guid WarehouseId { get; set; }

    /// <summary>StockDocumentId</summary>
    public Guid? StockDocumentId { get; set; }

    /// <summary>ReceiptNo</summary>
    public string ReceiptNo { get; set; } = string.Empty;

    /// <summary>ReceiptDate</summary>
    public DateTime ReceiptDate { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;
}
