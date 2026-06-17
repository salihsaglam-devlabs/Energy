using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.Procurement.SupplierQuote.Requests;

/// <summary>SupplierQuote oluşturma isteği.</summary>
public class CreateSupplierQuoteRequest
{
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
