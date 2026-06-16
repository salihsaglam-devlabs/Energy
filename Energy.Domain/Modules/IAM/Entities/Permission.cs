namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Tek bir yetki kodu (Module.Action) için katalog kaydı. Uygulama açılışında
/// <c>Energy.Shared.Identity.Permissions.PermissionCatalog</c> üzerinden seed
/// edilir ve arayüzden asla oluşturulup düzenlenmez.
/// </summary>
public class Permission
{
    /// <summary>Doğal birincil anahtar; ör. <c>User.Read</c>.</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Yetkinin ait olduğu modül (ör. "User").</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>Yetkinin eylemi (ör. "Read", "Create").</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Görünen ad için yerelleştirme (localization) anahtarı.</summary>
    public string DisplayNameKey { get; set; } = string.Empty;

    /// <summary>Açıklama için yerelleştirme anahtarı (opsiyonel).</summary>
    public string? DescriptionKey { get; set; }
}
