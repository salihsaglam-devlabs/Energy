using Energy.Domain.Common;

namespace Energy.Domain.System;

/// <summary>
/// Navigation node. Visibility is computed solely from
/// <see cref="RequiredPermissionCode"/>; there is no role↔menu join.
/// </summary>
public class Menu : AuditableEntity
{
    public Guid? ParentId { get; set; }

    /// <summary>Localization key for the display name.</summary>
    public string NameKey { get; set; } = string.Empty;

    /// <summary>NULL for pure container nodes.</summary>
    public string? Url { get; set; }
    public string? Icon { get; set; }
    public int DisplayOrder { get; set; }

    public bool IsVisible { get; set; } = true;
    public bool IsActive { get; set; } = true;

    /// <summary>NULL = visible to everyone (anonymous includes).</summary>
    public string? RequiredPermissionCode { get; set; }
}

