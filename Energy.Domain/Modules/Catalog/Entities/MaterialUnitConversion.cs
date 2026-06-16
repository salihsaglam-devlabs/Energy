using Energy.Domain.Common;

namespace Energy.Domain.Modules.Catalog;

/// <summary>Malzemeye özel birim dönüşümü.</summary>
public class MaterialUnitConversion : AuditableEntity
{
    public Guid MaterialId { get; set; }
    public Guid FromUnitOfMeasureId { get; set; }
    public Guid ToUnitOfMeasureId { get; set; }
    public decimal Factor { get; set; }
}
