namespace Energy.Shared.Models.V1.System.Responses;

public sealed class MenuResponse
{
    public Guid Id { get; init; }

    /// <summary>
    /// Display name resolved for the current request culture. When the stored
    /// <see cref="NameKey"/> matches a localization resource, this contains the
    /// translated value; otherwise it falls back to the raw key.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The raw localization key stored in the database (e.g. <c>Menus.System.Users</c>).
    /// Clients that perform their own translation should use this value.
    /// </summary>
    public string NameKey { get; init; } = string.Empty;

    public string Url { get; init; } = string.Empty;

    public string Icon { get; init; } = string.Empty;

    public int Order { get; init; }

    public Guid? ParentId { get; init; }

    /// <summary>
    /// Permission codes that must exist on the current user to access this
    /// menu. Empty means no extra permission gate besides role-menu assignment.
    /// </summary>
    public IReadOnlyList<string> RequiredPermissions { get; init; } = [];

    /// <summary>
    /// Populated when the response is returned from the hierarchical tree
    /// endpoint. Empty for flat list / detail endpoints.
    /// </summary>
    public IReadOnlyList<MenuResponse> Children { get; init; } = [];
}
