using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>Belge numarası üretim tanımı (modül bazlı).</summary>
public class SequenceDefinition : AuditableEntity
{
    public string Module { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string Prefix { get; set; } = string.Empty;
    public int Padding { get; set; } = 6;
    public long NextNumber { get; set; } = 1;
    public string? Format { get; set; }
}
