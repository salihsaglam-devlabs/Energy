using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Yetkilerin asıl sahibi olan rol. Yetkilerle eşleşen tek varlık budur.
/// </summary>
public class Role : AuditableEntity
{
    /// <summary>Rol adı (benzersiz).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Rol açıklaması (opsiyonel; yerelleştirme anahtarı tutabilir).</summary>
    public string? Description { get; set; }

    /// <summary>
    /// Yerleşik (built-in) roller için true (ör. SuperAdmin). Sistem rolleri
    /// yeniden adlandırılamaz veya silinemez; SuperAdmin ayrıca tüm yetki
    /// denetimlerini atlar (bypass).
    /// </summary>
    public bool IsSystem { get; set; }
}
