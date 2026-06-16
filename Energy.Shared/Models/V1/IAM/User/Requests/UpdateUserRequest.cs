namespace Energy.Shared.Models.V1.IAM.User.Requests;

/// <summary>User güncelleme isteği.</summary>
public class UpdateUserRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
