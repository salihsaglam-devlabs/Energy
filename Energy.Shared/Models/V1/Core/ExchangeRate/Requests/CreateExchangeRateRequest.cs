namespace Energy.Shared.Models.V1.Core.ExchangeRate.Requests;

/// <summary>ExchangeRate oluşturma isteği.</summary>
public class CreateExchangeRateRequest
{
    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>RateDate</summary>
    public DateTime RateDate { get; set; }

    /// <summary>Rate</summary>
    public decimal Rate { get; set; }
}
