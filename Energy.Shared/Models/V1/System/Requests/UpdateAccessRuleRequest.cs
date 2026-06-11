namespace Energy.Shared.Models.V1.System.Requests;

public sealed class UpdateAccessRuleRequest
{
    public string Name { get; init; } = string.Empty;

    public string Scope { get; init; } = string.Empty;

    public string Path { get; init; } = string.Empty;

    public string? HttpMethod { get; init; }

    public string? Description { get; init; }

    public bool IsEnabled { get; init; }
}

