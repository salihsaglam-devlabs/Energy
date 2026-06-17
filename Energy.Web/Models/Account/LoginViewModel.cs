namespace Energy.Web.Models.Account;

/// <summary>Giriş ekranını besleyen görünüm modeli.</summary>
public sealed class LoginViewModel
{
    /// <summary>Başarılı girişten sonra dönülecek yol.</summary>
    public string? ReturnUrl { get; init; }

    /// <summary>
    /// Yalnızca Geliştirme ortamında gösterilen hızlı giriş hazır ayarları; böylece
    /// bilinen tohum hesapları kimlik bilgileri tekrar yazılmadan seçilebilir.
    /// Geliştirme dışı her ortamda boştur (URL parametresiyle açılması hariç).
    /// </summary>
    public IReadOnlyList<DevAccount> DevAccounts { get; init; } = Array.Empty<DevAccount>();

    /// <summary>
    /// Geliştirme dışı ortamda hızlı girişi açan URL parametresinin değeri. POST
    /// sonrası (örn. doğrulama hatası) hızlı girişin görünür kalması için forma
    /// gizli alan olarak geri yazılır.
    /// </summary>
    public string? DevLoginToken { get; init; }
}

/// <summary>Tek tıkla geliştirme girişi için sunulan, tohumlanmış bir demo hesap.</summary>
public sealed record DevAccount(string Label, string UserName, string Password);

/// <summary>
/// Tohumlanmış demo hesapların kataloğu (altyapıdaki <c>SystemSeeder</c> ile senkron
/// tutulur). Yalnızca Geliştirme'de giriş sayfasında gösterilir.
/// </summary>
public static class DevLoginAccounts
{
    /// <summary>Tüm tohumlanmış demo hesaplar.</summary>
    public static readonly IReadOnlyList<DevAccount> All =
    [
        new("Admin — SuperAdmin", "admin", "Admin123!"),
        new("System Admin", "system.admin", "SysAdmin123!"),
        new("Operations Manager", "ops.manager", "OpsMgr123!"),
        new("Security Auditor", "security.auditor", "Auditor123!"),
        new("Localization Editor", "localization.editor", "Editor123!"),
        new("Read-only Viewer", "readonly.viewer", "Viewer123!"),
        new("Basic User", "basic.user", "Basic123!"),
    ];
}

