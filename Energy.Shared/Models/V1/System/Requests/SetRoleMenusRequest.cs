namespace Energy.Shared.Models.V1.System.Requests;

public sealed class SetRoleMenusRequest
{
    public IReadOnlyCollection<Guid> MenuIds { get; init; } = Array.Empty<Guid>();
}

