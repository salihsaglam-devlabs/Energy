using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Procurement.SupplierQuote.Responses;

/// <summary>SupplierQuote liste satırı.</summary>
public class SupplierQuoteListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
