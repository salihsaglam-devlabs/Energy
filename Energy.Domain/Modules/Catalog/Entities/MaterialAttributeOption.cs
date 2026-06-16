using Energy.Domain.Common;

namespace Energy.Domain.Modules.Catalog;

/// <summary>
/// Seçimli öznitelik değerleri
/// </summary>
public class MaterialAttributeOption : AuditableEntity
{
    /// <summary>MaterialAttributeDefinitions referansı</summary>
    public Guid MaterialAttributeDefinitionId { get; set; }

    /// <summary>Value</summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>DisplayOrder</summary>
    public int DisplayOrder { get; set; }
}
