using Energy.Domain.Common;

namespace Energy.Domain.Modules.Catalog;

/// <summary>Kategori ↔ öznitelik bağlantısı.</summary>
public class MaterialCategoryAttribute : AuditableEntity
{
    public Guid MaterialCategoryId { get; set; }
    public Guid MaterialAttributeDefinitionId { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
}
