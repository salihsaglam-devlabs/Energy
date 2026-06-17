using Energy.Domain.Common;

namespace Energy.Domain.Core;

/// <summary>Sistem genel ayarı (anahtar/değer).</summary>
public class SystemSetting : AuditableEntity
{
    public string Key { get; set; } = string.Empty;
    public string? Value { get; set; }
    public string? Category { get; set; }
    public string? DescriptionKey { get; set; }
}
