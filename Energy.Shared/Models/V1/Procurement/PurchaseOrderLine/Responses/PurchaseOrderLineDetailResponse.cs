namespace Energy.Shared.Models.V1.Procurement.PurchaseOrderLine.Responses;

/// <summary>PurchaseOrderLine detay görünümü.</summary>
public class PurchaseOrderLineDetailResponse
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

    /// <summary>Sipariş</summary>
    public Guid PurchaseOrderId { get; set; }

    /// <summary>Talep satırı</summary>
    public Guid? RequestLineId { get; set; }

    /// <summary>Malzeme</summary>
    public Guid? MaterialId { get; set; }

    /// <summary>Miktar</summary>
    public decimal Quantity { get; set; }

    /// <summary>Fiyat</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>Para birimi</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>ReceivedQuantity</summary>
    public decimal ReceivedQuantity { get; set; }
}
