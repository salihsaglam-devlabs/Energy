using Energy.Domain.Common;

namespace Energy.Domain.Modules.Catalog;

/// <summary>
/// Dinamik malzeme öznitelik tanımları
/// </summary>
public class MaterialAttributeDefinition : AuditableEntity
{
    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>DataType</summary>
    public string DataType { get; set; } = string.Empty;

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
