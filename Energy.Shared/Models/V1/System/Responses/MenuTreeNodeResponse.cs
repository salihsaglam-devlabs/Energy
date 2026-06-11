namespace Energy.Shared.Models.V1.System.Responses;

public sealed class MenuTreeNodeResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Url { get; init; }
    public string? Icon { get; init; }
    public int DisplayOrder { get; init; }
    public IReadOnlyList<MenuTreeNodeResponse> Children { get; init; } = Array.Empty<MenuTreeNodeResponse>();
}
