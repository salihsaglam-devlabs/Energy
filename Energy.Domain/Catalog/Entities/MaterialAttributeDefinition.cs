using Energy.Domain.Common;

namespace Energy.Domain.Catalog;

/// <summary>Dinamik malzeme öznitelik tanımı.</summary>
public class MaterialAttributeDefinition : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Text, Number, Decimal, Boolean, Date, Option.</summary>
    public string DataType { get; set; } = "Text";
    public bool IsActive { get; set; } = true;
}
