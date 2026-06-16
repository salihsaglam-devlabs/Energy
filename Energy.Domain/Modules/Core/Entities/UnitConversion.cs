using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>Genel birim dönüşümü (malzemeden bağımsız).</summary>
public class UnitConversion : AuditableEntity
{
    public Guid FromUnitOfMeasureId { get; set; }
    public Guid ToUnitOfMeasureId { get; set; }
    public decimal Factor { get; set; }
}
