namespace Energy.Shared.Models.V1.Procurement.PurchaseOrder.Responses;

/// <summary>PurchaseOrder detay görünümü.</summary>
public class PurchaseOrderDetailResponse
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

    /// <summary>Tedarikçi</summary>
    public Guid SupplierId { get; set; }

    /// <summary>Opsiyonel proje</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Durum</summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>Sipariş no</summary>
    public string OrderNo { get; set; } = string.Empty;

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>OrderDate</summary>
    public DateTime OrderDate { get; set; }

    /// <summary>ApprovalRequestId</summary>
    public Guid? ApprovalRequestId { get; set; }
}
