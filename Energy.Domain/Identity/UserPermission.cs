namespace Energy.Domain.Identity;

/// <summary>
/// Doğrudan kullanıcı→yetki ataması. Kullanıcının rolleri üzerinden zaten
/// devraldığı yetkilerin ÜZERİNE eklenir. Satır silindiğinde yalnızca bu
/// doğrudan atama kaldırılır; rol üzerinden gelen yetkiler etkilenmez.
/// </summary>
public class UserPermission
{
    /// <summary>Yetkinin doğrudan atandığı kullanıcının kimliği.</summary>
    public Guid UserId { get; set; }

    /// <summary>Kullanıcıya doğrudan atanan yetki kodu.</summary>
    public string PermissionCode { get; set; } = string.Empty;
}
