namespace Energy.Web.Services.Navigation;

/// <summary>
/// View model used by the drawer's <c>dxTreeView</c>. The Web layer flattens
/// the API menu tree into this shape after applying user-permission filters.
/// </summary>
public sealed class NavigationItem
{
    public required Guid Id { get; init; }

    public Guid? ParentId { get; init; }

    public required string Name { get; init; }

    public string? Url { get; init; }

    public string? Icon { get; init; }

    public int Order { get; init; }

    public IReadOnlyList<string> RequiredPermissions { get; init; } = [];
}

