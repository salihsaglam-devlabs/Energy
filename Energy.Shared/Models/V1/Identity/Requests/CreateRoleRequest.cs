namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class CreateRoleRequest
{
    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;
}
