using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>
/// Belge numarası üretim tanımları
/// </summary>
public class SequenceDefinition : AuditableEntity
{
    /// <summary>Module</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>EntityType</summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>Prefix</summary>
    public string Prefix { get; set; } = string.Empty;

    /// <summary>Padding</summary>
    public int Padding { get; set; }

    /// <summary>NextNumber</summary>
    public long NextNumber { get; set; }

    /// <summary>Format</summary>
    public string? Format { get; set; }
}
