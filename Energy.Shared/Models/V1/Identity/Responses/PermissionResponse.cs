namespace Energy.Shared.Models.V1.Identity.Responses;

/// <summary>Tek bir yetki (permission) tanımının ve kullanım sayaçlarının görünümü.</summary>
public sealed class PermissionResponse
{
    /// <summary>Yetkinin benzersiz kodu (örn. "User.ReadAll").</summary>
    public string Code { get; init; } = string.Empty;

    /// <summary>Yetkinin ait olduğu modül.</summary>
    public string Module { get; init; } = string.Empty;

    /// <summary>Yetkinin temsil ettiği eylem.</summary>
    public string Action { get; init; } = string.Empty;

    /// <summary>Arayüzde gösterilecek ad.</summary>
    public string DisplayName { get; init; } = string.Empty;

    /// <summary>İsteğe bağlı açıklama.</summary>
    public string? Description { get; init; }

    /// <summary>Bu yetkiye sahip rol sayısı.</summary>
    public int RoleCount { get; init; }

    /// <summary>Bu yetkiyi gerektiren menü sayısı.</summary>
    public int MenuCount { get; init; }

    /// <summary>Bu yetkiyi gerektiren API uç noktası sayısı.</summary>
    public int EndpointCount { get; init; }
}
