namespace Energy.Shared.Models.V1.Identity.Requests;

public sealed class CreatePermissionRequest
{
    public string Code { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;
}

