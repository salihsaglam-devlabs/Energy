namespace Energy.Web.Services.Navigation;

public sealed class NavigationItem
{
    public Guid Id { get; init; }
    public Guid? ParentId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Url { get; init; }
    public string? Icon { get; init; }
    public int Order { get; init; }
}
