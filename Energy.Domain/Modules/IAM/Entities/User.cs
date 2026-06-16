using Energy.Domain.Common;

namespace Energy.Domain.Modules.IAM;

/// <summary>
/// Kullanıcı hesapları
/// </summary>
public class User : AuditableEntity
{
    /// <summary>Kullanıcı adı</summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>E-posta</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Ad</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Soyad</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>Aktiflik</summary>
    public bool IsActive { get; set; }
}
