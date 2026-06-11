using Energy.Domain.Common;

namespace Energy.Domain.System;

/// <summary>
/// Catalog of every protected API endpoint and the single permission it
/// requires. Replaces the legacy AccessRule + AccessRulePermission pair.
/// </summary>
public class ApiEndpoint : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    /// <summary>Route template, e.g. <c>/api/v1/users/{id}</c>.</summary>
    public string Path { get; set; } = string.Empty;
    public string HttpMethod { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    /// <summary>NULL = anonymous (no permission required).</summary>
    public string? RequiredPermissionCode { get; set; }
}

