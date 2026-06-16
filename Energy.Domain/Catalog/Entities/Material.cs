using Energy.Domain.Common;

namespace Energy.Domain.Catalog;

/// <summary>Malzeme kartı.</summary>
public class Material : AuditableEntity
{
    public Guid MaterialCategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public Guid BaseUnitOfMeasureId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsBatchTracked { get; set; }
    public bool IsSerialTracked { get; set; }
    public bool IsActive { get; set; }
}
