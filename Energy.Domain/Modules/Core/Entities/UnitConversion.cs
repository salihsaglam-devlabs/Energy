using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>
/// Birim dönüşümleri
/// </summary>
public class UnitConversion : AuditableEntity
{
    /// <summary>FromUnitOfMeasureId</summary>
    public Guid FromUnitOfMeasureId { get; set; }

    /// <summary>ToUnitOfMeasureId</summary>
    public Guid ToUnitOfMeasureId { get; set; }

    /// <summary>Factor</summary>
    public decimal Factor { get; set; }
}
