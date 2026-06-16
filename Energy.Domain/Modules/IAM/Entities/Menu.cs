using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Navigasyon (menü) düğümü. Görünürlük yalnızca
/// <see cref="RequiredPermissionCode"/> üzerinden hesaplanır; rol↔menü bağlantısı
/// yoktur.
/// </summary>
public class Menu : AuditableEntity
{
    /// <summary>Üst menü düğümünün kimliği; kök düğümlerde null.</summary>
    public Guid? ParentId { get; set; }

    /// <summary>Görünen ad için yerelleştirme anahtarı.</summary>
    public string NameKey { get; set; } = string.Empty;

    /// <summary>Yalnızca kapsayıcı (container) düğümlerde NULL.</summary>
    public string? Url { get; set; }

    /// <summary>Menü ikonu (opsiyonel).</summary>
    public string? Icon { get; set; }

    /// <summary>Kardeş düğümler arasındaki görüntüleme sırası.</summary>
    public int DisplayOrder { get; set; }

    /// <summary>Menünün görünür olup olmadığı.</summary>
    public bool IsVisible { get; set; } = true;

    /// <summary>Menünün aktif olup olmadığı.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>NULL = herkese görünür (anonim dahil).</summary>
    public string? RequiredPermissionCode { get; set; }
}
