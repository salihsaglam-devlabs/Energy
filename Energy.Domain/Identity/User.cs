using Energy.Domain.Common;

namespace Energy.Domain.Identity;

/// <summary>
/// Sistem kullanıcısı. Yetkiler yalnızca roller üzerinden türetilir; bu varlık
/// doğrudan yetki durumu taşımaz.
/// </summary>
public class User : AuditableEntity
{
    /// <summary>Kullanıcı adı (benzersiz).</summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>E-posta adresi (benzersiz).</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Parolanın hash'lenmiş hali (asla düz metin saklanmaz).</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>Kullanıcının adı.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Kullanıcının soyadı.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Hesabın aktif olup olmadığı; pasif hesaplar giriş yapamaz.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Kimlik bilgileri, roller veya aktiflik durumu değiştiğinde yenilenir.
    /// Her istekte doğrulanır; böylece eski token'lar anında geçersiz kılınır.
    /// </summary>
    public Guid SecurityStamp { get; set; } = Guid.NewGuid();

    /// <summary>Son başarılı giriş zamanı (UTC).</summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>Ardışık başarısız giriş denemesi sayısı (kilitleme için).</summary>
    public int FailedLoginCount { get; set; }

    /// <summary>Hesap kilidi bitiş zamanı; null ise hesap kilitli değildir.</summary>
    public DateTime? LockoutEnd { get; set; }

    /// <summary>Profil resminin ham byte içeriği (opsiyonel).</summary>
    public byte[]? ProfileImage { get; set; }

    /// <summary>Profil resminin MIME türü (ör. image/png).</summary>
    public string? ProfileImageContentType { get; set; }
}
