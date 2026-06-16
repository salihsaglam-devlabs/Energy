using Energy.Domain.Common;

namespace Energy.Domain.Modules.Catalog;

/// <summary>
/// Kategori öznitelik bağlantıları
/// </summary>
public class MaterialCategoryAttribute : AuditableEntity
{
    /// <summary>MaterialCategories referansı</summary>
    public Guid MaterialCategoryId { get; set; }

    /// <summary>MaterialAttributeDefinitions referansı</summary>
    public Guid MaterialAttributeDefinitionId { get; set; }

    /// <summary>IsRequired</summary>
    public bool IsRequired { get; set; }

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }
}
