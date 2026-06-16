using Energy.Domain.Common;

namespace Energy.Domain.Modules.Catalog;

/// <summary>
/// Malzemeye özel birim dönüşümleri
/// </summary>
public class MaterialUnitConversion : AuditableEntity
{
    /// <summary>MaterialId</summary>
    public Guid MaterialId { get; set; }

    /// <summary>FromUnitOfMeasureId</summary>
    public Guid FromUnitOfMeasureId { get; set; }

    /// <summary>ToUnitOfMeasureId</summary>
    public Guid ToUnitOfMeasureId { get; set; }

    /// <summary>Factor</summary>
    public decimal Factor { get; set; }
}
