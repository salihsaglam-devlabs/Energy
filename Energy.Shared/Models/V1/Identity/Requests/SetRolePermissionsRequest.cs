namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class SetRolePermissionsRequest
{
    public IReadOnlyCollection<string> PermissionCodes { get; set; } = Array.Empty<string>();
}

