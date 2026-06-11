namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class AdminPermissionHealthResponse
{
    public bool AdminRoleExists { get; init; }

    public int TotalPermissions { get; init; }

    public int AssignedPermissions { get; init; }

    public IReadOnlyList<string> MissingPermissionCodes { get; init; } = Array.Empty<string>();

    public bool HasAllPermissions => AdminRoleExists && MissingPermissionCodes.Count == 0;
}

