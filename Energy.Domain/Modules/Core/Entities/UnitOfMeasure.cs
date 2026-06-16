using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>Ölçü birimi.</summary>
public class UnitOfMeasure : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public bool IsActive { get; set; } = true;
}
