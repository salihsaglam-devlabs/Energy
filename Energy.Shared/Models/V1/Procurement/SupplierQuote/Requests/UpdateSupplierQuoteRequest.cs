namespace Energy.Shared.Models.V1.Procurement.SupplierQuote.Requests;

/// <summary>SupplierQuote güncelleme isteği.</summary>
public class UpdateSupplierQuoteRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
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
    public string Status { get; set; } = string.Empty;

    /// <summary>IsSelected</summary>
    public bool IsSelected { get; set; }
}
