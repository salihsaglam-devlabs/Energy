namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>Bir kullanıcının parolasını değiştirmek için kullanılan istek.</summary>
public sealed class ChangePasswordRequest
{
    /// <summary>Kullanıcının yeni parolası.</summary>
    public string NewPassword { get; set; } = string.Empty;
}
