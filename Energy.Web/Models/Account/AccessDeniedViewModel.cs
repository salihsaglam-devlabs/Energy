namespace Energy.Web.Models.Account;

/// <summary>Erişim reddedildi ekranını besleyen görünüm modeli.</summary>
public sealed class AccessDeniedViewModel
{
    /// <summary>Erişimin reddedildiği istek yolu.</summary>
    public string RequestedPath { get; init; } = "/";

    /// <summary>
    /// Reddedilen sayfanın/uç noktanın gerektirdiği tam yetki kodu. Erişim reddedildi
    /// ekranına doğrudan ulaşıldığında (belirli bir yetki söz konusu olmadığında) null
    /// olur; bu durumda görünüm yetki satırını gizler — eski "Default.ReadAll"
    /// yer tutucusu gibi uydurma, katalog dışı bir kod asla gösterilmez.
    /// </summary>
    public string? RequestedPermission { get; init; }
}

