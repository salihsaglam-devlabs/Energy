namespace Energy.Shared.Models.V1.System.Responses;

public sealed class AccessRuleResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Scope { get; init; } = string.Empty;

    public string Path { get; init; } = string.Empty;

    public string HttpMethod { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public bool IsEnabled { get; init; }
}

