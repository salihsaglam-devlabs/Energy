namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class RoleDetailResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsSystem { get; init; }
    public IReadOnlyCollection<string> PermissionCodes { get; init; } = Array.Empty<string>();
    public IReadOnlyCollection<UserSummaryResponse> Users { get; init; } = Array.Empty<UserSummaryResponse>();
}
