using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>
/// Sistem genel ayarları
/// </summary>
public class SystemSetting : AuditableEntity
{
    /// <summary>Key</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Value</summary>
    public string? Value { get; set; }

    /// <summary>Category</summary>
    public string? Category { get; set; }

    /// <summary>DescriptionKey</summary>
    public string? DescriptionKey { get; set; }
}
