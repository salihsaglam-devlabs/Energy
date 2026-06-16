using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>Kur kaydı. Belirli bir tarihte bir para biriminin ana para birimine oranı.</summary>
public class ExchangeRate : AuditableEntity
{
    public Guid CurrencyId { get; set; }
    public DateTime RateDate { get; set; }
    public decimal Rate { get; set; }
}
