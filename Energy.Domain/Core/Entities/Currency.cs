using Energy.Domain.Common;

namespace Energy.Domain.Core;

/// <summary>Para birimi.</summary>
public class Currency : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Symbol { get; set; }
    public bool IsActive { get; set; } = true;
}
