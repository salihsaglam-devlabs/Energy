namespace Energy.Shared.Models.V1.Home.Responses;

/// <summary>Ana sayfa gösterge panosunda gösterilen özet istatistikler.</summary>
public sealed class HomeDashboardResponse
{
    /// <summary>Etkin kullanıcı sayısı.</summary>
    public int ActiveUsers { get; init; }

    /// <summary>Toplam rol sayısı.</summary>
    public int TotalRoles { get; init; }

    /// <summary>Toplam yetki (permission) sayısı.</summary>
    public int TotalPermissions { get; init; }

    /// <summary>Toplam menü sayısı.</summary>
    public int TotalMenus { get; init; }

    /// <summary>Toplam API uç noktası sayısı.</summary>
    public int TotalApiEndpoints { get; init; }

    /// <summary>Son 24 saatteki başarısız giriş denemesi sayısı.</summary>
    public int FailedLogins24h { get; init; }
}
