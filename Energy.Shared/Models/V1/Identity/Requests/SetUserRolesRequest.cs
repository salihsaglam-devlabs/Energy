namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class SetUserRolesRequest
{
    public IReadOnlyList<Guid> RoleIds { get; init; } = [];
}
