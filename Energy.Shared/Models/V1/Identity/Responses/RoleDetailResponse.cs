namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class RoleDetailResponse
{
    public Guid Id { get; init; }

    public string? Name { get; init; }

    public string? NormalizedName { get; init; }

    public string Description { get; init; } = string.Empty;

    public int AssignedUserCount { get; init; }
}
