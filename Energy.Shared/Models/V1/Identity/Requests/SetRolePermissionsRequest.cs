namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class SetRolePermissionsRequest
{
    public IReadOnlyCollection<Guid> PermissionIds { get; init; } = Array.Empty<Guid>();
}

