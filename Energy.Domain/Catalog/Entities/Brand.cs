using Energy.Domain.Common;

namespace Energy.Domain.Catalog;

/// <summary>Marka.</summary>
public class Brand : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
