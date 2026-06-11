namespace Energy.Shared.Models.V1.Identity.Responses;

public sealed class PermissionResponse
{
    public Guid Id { get; init; }

    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
}

