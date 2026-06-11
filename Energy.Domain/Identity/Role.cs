using Energy.Domain.Common;

namespace Energy.Domain.Identity;

/// <summary>
/// Authoritative permission owner. The only entity that maps to permissions.
/// </summary>
public class Role : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    /// <summary>
    /// True for built-in roles (e.g. SuperAdmin). System roles cannot be
    /// renamed or deleted; SuperAdmin additionally bypasses permission checks.
    /// </summary>
    public bool IsSystem { get; set; }
}

