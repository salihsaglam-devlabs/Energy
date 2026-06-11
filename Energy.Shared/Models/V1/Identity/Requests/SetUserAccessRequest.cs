namespace Energy.Shared.Models.V1.Identity.Requests;

/// <summary>
/// Replaces a user's full access in one shot: the desired role set and the
/// desired direct (per-user) permission grants. Unknown permission codes are
/// ignored by the service; permissions already inherited from roles do not need
/// to be repeated here.
/// </summary>
public sealed class SetUserAccessRequest
{
    public IReadOnlyList<Guid> RoleIds { get; init; } = Array.Empty<Guid>();
    public IReadOnlyList<string> DirectPermissionCodes { get; init; } = Array.Empty<string>();
}

