using Energy.Domain.Common;

namespace Energy.Domain.Identity;

/// <summary>
/// System user. Permissions are derived only through roles; this entity
/// carries no permission state directly.
/// </summary>
public class User : AuditableEntity
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Rotated whenever credentials, roles, or activation change. Validated on
    /// every request so old tokens are invalidated immediately.
    /// </summary>
    public Guid SecurityStamp { get; set; } = Guid.NewGuid();

    public DateTime? LastLoginAt { get; set; }
    public int FailedLoginCount { get; set; }
    public DateTime? LockoutEnd { get; set; }

    public byte[]? ProfileImage { get; set; }
    public string? ProfileImageContentType { get; set; }
}

