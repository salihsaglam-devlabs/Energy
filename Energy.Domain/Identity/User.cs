using Energy.Domain.Common;

namespace Energy.Domain.Identity;

public class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    /// <summary>
    /// Raw binary content of the user's profile image. Stored in the database
    /// as <c>bytea</c> so the system has no external dependency for avatars.
    /// </summary>
    public byte[]? ProfileImage { get; set; }

    /// <summary>
    /// MIME type of <see cref="ProfileImage"/> (e.g. <c>image/png</c>). Required
    /// so the avatar can be served with the correct <c>Content-Type</c> header.
    /// </summary>
    public string? ProfileImageContentType { get; set; }
}
