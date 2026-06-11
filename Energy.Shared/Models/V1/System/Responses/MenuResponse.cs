namespace Energy.Shared.Models.V1.System.Responses;

public sealed class MenuResponse
{
    public Guid Id { get; init; }
    public Guid? ParentId { get; init; }
    public string NameKey { get; init; } = string.Empty;
    public string? Url { get; init; }
    public string? Icon { get; init; }
    public int DisplayOrder { get; init; }
    public bool IsVisible { get; init; }
    public bool IsActive { get; init; }
    public string? RequiredPermissionCode { get; init; }
}
