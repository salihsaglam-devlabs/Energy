namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>Yeni bir kullanıcı oluşturmak için kullanılan istek.</summary>
public sealed class CreateUserRequest
{
    /// <summary>Kullanıcı adı.</summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>E-posta adresi.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Adı.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Soyadı.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>İlk parola.</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>Kullanıcı etkin olarak mı oluşturulsun.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Kullanıcıya atanacak rollerin kimlikleri.</summary>
    public IReadOnlyCollection<Guid> RoleIds { get; set; } = Array.Empty<Guid>();
}
