using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>
/// Kur kayıtları
/// </summary>
public class ExchangeRate : AuditableEntity
{
    /// <summary>CurrencyId</summary>
    public Guid CurrencyId { get; set; }

    /// <summary>RateDate</summary>
    public DateTime RateDate { get; set; }

    /// <summary>Rate</summary>
    public decimal Rate { get; set; }
}
