namespace Energy.Shared.Models.V1.IAM.User.Requests;

/// <summary>User oluşturma isteği.</summary>
public class CreateUserRequest
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
