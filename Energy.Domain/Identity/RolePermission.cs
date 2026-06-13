namespace Energy.Domain.Identity;

/// <summary>
/// "Kim neyi yapabilir" sorusunun TEK doğruluk kaynağı. Başka hiçbir tablo
/// yetkileri rollere/kullanıcılara eşlemez.
/// </summary>
public class RolePermission
{
    /// <summary>Yetkinin atandığı rolün kimliği.</summary>
    public Guid RoleId { get; set; }

    /// <summary>Role atanan yetki kodu (ör. <c>User.Read</c>).</summary>
    public string PermissionCode { get; set; } = string.Empty;
}
