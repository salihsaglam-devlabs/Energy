using Energy.Domain.Common;

namespace Energy.Domain.Modules.Core;

/// <summary>
/// Çok dilli metin kaynakları
/// </summary>
public class LocalizationResource : AuditableEntity
{
    /// <summary>Alternatif anahtar</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Culture</summary>
    public string Culture { get; set; } = string.Empty;

    /// <summary>Value</summary>
    public string Value { get; set; } = string.Empty;
}
