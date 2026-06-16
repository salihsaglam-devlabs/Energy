namespace Energy.Shared.Models.V1.Core.ExchangeRate.Responses;

/// <summary>ExchangeRate liste satırı.</summary>
public class ExchangeRateListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>RateDate</summary>
    public DateTime RateDate { get; set; }

    /// <summary>Rate</summary>
    public decimal Rate { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
