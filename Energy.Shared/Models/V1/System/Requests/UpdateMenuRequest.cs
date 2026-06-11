namespace Energy.Shared.Models.V1.System.Requests;

public sealed class UpdateMenuRequest
{
    public string Name { get; init; } = string.Empty;

    public string Url { get; init; } = string.Empty;

    public string Icon { get; init; } = string.Empty;

    public int Order { get; init; }

    public Guid? ParentId { get; init; }
}

