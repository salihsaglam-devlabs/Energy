namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class UpdatePermissionRequest
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
}

