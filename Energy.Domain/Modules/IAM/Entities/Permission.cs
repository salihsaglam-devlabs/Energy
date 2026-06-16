using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Permission kataloğu
/// </summary>
public class Permission : AuditableEntity
{
    /// <summary>Permission kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Modül</summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>İşlem</summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>Görünen ad anahtarı</summary>
    public string DisplayNameKey { get; set; } = string.Empty;
}
