namespace Energy.Shared.Models.V1.Procurement.SupplierQuoteLine.Responses;

/// <summary>SupplierQuoteLine detay görünümü.</summary>
public class SupplierQuoteLineDetailResponse
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

    /// <summary>SupplierQuotes referansı</summary>
    public Guid SupplierQuoteId { get; set; }

    /// <summary>RequestLines referansı</summary>
    public Guid? RequestLineId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid? MaterialId { get; set; }

    /// <summary>Description</summary>
    public string? Description { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>UnitPrice</summary>
    public decimal UnitPrice { get; set; }

    /// <summary>TaxRate</summary>
    public decimal TaxRate { get; set; }

    /// <summary>DiscountRate</summary>
    public decimal DiscountRate { get; set; }

    /// <summary>DeliveryDays</summary>
    public int DeliveryDays { get; set; }
}
