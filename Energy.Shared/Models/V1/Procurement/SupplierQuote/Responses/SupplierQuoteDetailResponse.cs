using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;

/// <summary>SupplierQuote detay görünümü.</summary>
public class SupplierQuoteDetailResponse
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

    /// <summary>ProjectId</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>QuoteNo</summary>
    public string QuoteNo { get; set; } = string.Empty;

    /// <summary>QuoteDate</summary>
    public DateTime QuoteDate { get; set; }

    /// <summary>PaymentTerm</summary>
    public string? PaymentTerm { get; set; }

    /// <summary>Status</summary>
    public DocumentStatus Status { get; set; }

    /// <summary>IsSelected</summary>
    public bool IsSelected { get; set; }
}
