namespace Energy.Shared.Models.V1.Identity.Responses;

/// <summary>
/// A complete picture of a single user's access for the dedicated access
/// management screen: assigned roles, permissions inherited through those roles
/// (read-only), and direct per-user grants layered on top.
/// </summary>
public sealed class UserAccessResponse
{
    public Guid UserId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public bool IsActive { get; init; }

    /// <summary>Roles currently assigned to the user.</summary>
    public IReadOnlyList<Guid> RoleIds { get; init; } = Array.Empty<Guid>();

    /// <summary>Permission codes the user owns through their roles (inherited, read-only).</summary>
    public IReadOnlyList<string> RolePermissionCodes { get; init; } = Array.Empty<string>();

    /// <summary>Permission codes granted directly to the user, on top of role permissions.</summary>
    public IReadOnlyList<string> DirectPermissionCodes { get; init; } = Array.Empty<string>();
}

