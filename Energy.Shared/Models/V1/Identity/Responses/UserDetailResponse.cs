namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class UserDetailResponse
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public string? UserName { get; init; }

    public string? NormalizedUserName { get; init; }

    public string? Email { get; init; }

    public string? NormalizedEmail { get; init; }

    public bool EmailConfirmed { get; init; }

    public string? PhoneNumber { get; init; }

    public bool PhoneNumberConfirmed { get; init; }

    public bool TwoFactorEnabled { get; init; }

    public DateTimeOffset? LockoutEnd { get; init; }

    public bool LockoutEnabled { get; init; }

    public int AccessFailedCount { get; init; }

    /// <summary>Whether the user has uploaded a profile image.</summary>
    public bool HasProfileImage { get; init; }

    /// <summary>MIME type of the stored profile image, when present.</summary>
    public string? ProfileImageContentType { get; init; }

    public IReadOnlyList<RoleSummaryResponse> Roles { get; init; } = [];
}
