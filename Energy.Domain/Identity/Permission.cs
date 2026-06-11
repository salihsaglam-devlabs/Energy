namespace Energy.Domain.Identity;

/// <summary>
/// Catalog row for a single permission code (Module.Action). Seeded from
/// <c>Energy.Shared.Identity.Permissions.PermissionCatalog</c> at startup and
/// never created/edited from the UI.
/// </summary>
public class Permission
{
    /// <summary>Natural primary key, e.g. <c>User.Read</c>.</summary>
    public string Code { get; set; } = string.Empty;

    public string Module { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;

    public string DisplayNameKey { get; set; } = string.Empty;
    public string? DescriptionKey { get; set; }
}

