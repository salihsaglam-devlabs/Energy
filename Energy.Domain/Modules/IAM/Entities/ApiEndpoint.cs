using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Korunan her API endpoint'inin ve gerektirdiği tek yetkinin kataloğu. Eski
/// AccessRule + AccessRulePermission ikilisinin yerini alır.
/// </summary>
public class ApiEndpoint : AuditableEntity
{
    /// <summary>Endpoint adı (genellikle "Controller.Action" kuralıyla).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Açıklama (opsiyonel).</summary>
    public string? Description { get; set; }

    /// <summary>Rota şablonu; ör. <c>/api/v1/users/{id}</c>.</summary>
    public string Path { get; set; } = string.Empty;

    /// <summary>HTTP metodu (GET, POST, ...).</summary>
    public string HttpMethod { get; set; } = string.Empty;

    /// <summary>Endpoint'in aktif olup olmadığı; pasifse erişim reddedilir.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>NULL = anonim (yetki gerekmez).</summary>
    public string? RequiredPermissionCode { get; set; }
}
