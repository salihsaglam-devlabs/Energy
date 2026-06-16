using Energy.Domain.Common;

namespace Energy.Domain.Modules.Catalog;

/// <summary>
/// Malzeme kategori ağacı
/// </summary>
public class MaterialCategory : AuditableEntity
{
    /// <summary>ParentCategoryId</summary>
    public Guid? ParentCategoryId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
