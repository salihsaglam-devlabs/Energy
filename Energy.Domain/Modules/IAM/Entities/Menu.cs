using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Menü ağacı
/// </summary>
public class Menu : AuditableEntity
{
    /// <summary>Üst menü</summary>
    public Guid? ParentId { get; set; }

    /// <summary>Lokalizasyon anahtarı</summary>
    public string NameKey { get; set; } = string.Empty;

    /// <summary>URL</summary>
    public string? Url { get; set; }

    /// <summary>Gerekli permission</summary>
    public string? RequiredPermissionCode { get; set; }
}
