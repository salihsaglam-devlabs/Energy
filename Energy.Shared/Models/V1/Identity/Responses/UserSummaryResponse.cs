namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class UserSummaryResponse
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public string? UserName { get; init; }

    public string? Email { get; init; }

    public bool HasProfileImage { get; init; }

    public IReadOnlyList<RoleSummaryResponse> Roles { get; init; } = [];
}
