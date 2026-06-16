namespace Energy.Shared.Models.V1.Core.ExchangeRate.Requests;

/// <summary>ExchangeRate güncelleme isteği.</summary>
public class UpdateExchangeRateRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>RateDate</summary>
    public DateTime RateDate { get; set; }

    /// <summary>Rate</summary>
    public decimal Rate { get; set; }
}
