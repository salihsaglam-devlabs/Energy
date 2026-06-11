namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class PermissionResponse
{
    public string Code { get; init; } = string.Empty;
    public string Module { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int RoleCount { get; init; }
    public int MenuCount { get; init; }
    public int EndpointCount { get; init; }
}
