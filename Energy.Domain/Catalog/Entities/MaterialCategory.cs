using Energy.Domain.Common;

namespace Energy.Domain.Catalog;

/// <summary>Malzeme kategori ağacı.</summary>
public class MaterialCategory : AuditableEntity
{
    public Guid? ParentCategoryId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
