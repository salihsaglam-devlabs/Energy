namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class UpdateUserRequest
{
    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public string UserName { get; init; } = string.Empty;

    public string? Email { get; init; }

    public bool EmailConfirmed { get; init; }

    public string? PhoneNumber { get; init; }

    public bool PhoneNumberConfirmed { get; init; }

    public bool TwoFactorEnabled { get; init; }

    public bool LockoutEnabled { get; init; }

    public DateTimeOffset? LockoutEnd { get; init; }
}
