namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>Kullanıcı girişi için kimlik bilgilerini taşıyan istek.</summary>
public sealed class LoginRequest
{
    /// <summary>Kullanıcı adı veya e-posta adresi.</summary>
    public string UserNameOrEmail { get; set; } = string.Empty;

    /// <summary>Parola.</summary>
    public string Password { get; set; } = string.Empty;
}
