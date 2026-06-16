using Energy.Domain.Common;

namespace Energy.Domain.Catalog;

/// <summary>Seçimli (Option) öznitelik değeri.</summary>
public class MaterialAttributeOption : AuditableEntity
{
    public Guid MaterialAttributeDefinitionId { get; set; }
    public string Value { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
