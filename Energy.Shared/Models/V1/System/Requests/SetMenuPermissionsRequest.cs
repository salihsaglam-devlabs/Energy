namespace Energy.Shared.Models.V1.System.Requests;

public sealed class SetMenuPermissionsRequest
{
    public IReadOnlyCollection<Guid> PermissionIds { get; init; } = Array.Empty<Guid>();
}

