namespace Energy.Shared.Models.V1.IAM.User.Responses;

/// <summary>User liste satırı.</summary>
public class UserListResponse
{
    /// <summary>Kimlik.</summary>
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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
