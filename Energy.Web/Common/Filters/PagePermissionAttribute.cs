namespace Energy.Web.Common.Filters;

/// <summary>
/// Bir MVC sayfasını (controller veya action) görüntülemek için gereken yetki kodunu
/// bildirir. Girişte kimlik doğrulama çerezine yazılan yetki taleplerine (claims) karşı
/// <see cref="PageAccessFilter"/> tarafından zorlanır. API ile aynı
/// <see cref="Energy.Shared.Identity.Permissions.PermissionCatalog"/> kodlarını kullanır;
/// böylece adlandırma uçtan uca tutarlı kalır.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class PagePermissionAttribute : Attribute
{
    /// <summary>Gereken yetki koduyla özniteliği oluşturur.</summary>
    public PagePermissionAttribute(string permission) => Permission = permission;

    /// <summary>Sayfayı görüntülemek için gereken yetki kodu.</summary>
    public string Permission { get; }
}

