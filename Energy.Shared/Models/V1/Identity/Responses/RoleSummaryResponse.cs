namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class RoleSummaryResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsSystem { get; init; }
    public int PermissionCount { get; init; }
    public int UserCount { get; init; }
}
