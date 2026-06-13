namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>Var olan bir kullanıcıyı güncellemek için kullanılan istek.</summary>
public sealed class UpdateUserRequest
{
    /// <summary>Adı.</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>Soyadı.</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>E-posta adresi.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Kullanıcının etkin olup olmadığı.</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Kullanıcıya atanacak rollerin kimlikleri.</summary>
    public IReadOnlyCollection<Guid> RoleIds { get; set; } = Array.Empty<Guid>();
}
