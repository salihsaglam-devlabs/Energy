namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class SeedAdminResponse
{
    public Guid UserId { get; init; }

    public Guid RoleId { get; init; }

    public string Email { get; init; } = string.Empty;

    public string DefaultPassword { get; init; } = string.Empty;

    public bool UserCreated { get; init; }

    public bool RoleCreated { get; init; }
}

