using System.Security.Claims;

namespace Energy.Web.Common;

/// <summary>
/// Custom claim types used by the Web layer's cookie principal. These mirror
/// the JWT claims issued by the API (permission, role) plus a couple of
/// Web-only ones (role id, full name).
/// </summary>
public static class EnergyClaimTypes
{
    public const string Permission = "permission";

    /// <summary>
    /// Database identifier of a role the user belongs to. Stored alongside
    /// <see cref="ClaimTypes.Role"/> (which holds the role name) so the Web
    /// layer can call role-scoped API endpoints (e.g. <c>/roles/{id}/menus</c>)
    /// without an extra lookup.
    /// </summary>
    public const string RoleId = "role_id";

    /// <summary>
    /// Stable, culture-independent identifier for a role (its
    /// <c>NormalizedName</c>, e.g. <c>"ADMIN"</c>). Used to detect well-known
    /// roles regardless of how their display name is localized.
    /// </summary>
    public const string RoleKey = "role_key";

    public const string FullName = "full_name";
}

public static class ClaimsPrincipalExtensions
{
    public static bool HasPermission(this ClaimsPrincipal? principal, string permission)
    {
        if (principal is null || string.IsNullOrEmpty(permission))
        {
            return false;
        }

        return principal.HasClaim(EnergyClaimTypes.Permission, permission);
    }

    public static bool HasAnyPermission(this ClaimsPrincipal? principal, params string[] permissions)
    {
        if (principal is null || permissions.Length == 0)
        {
            return false;
        }

        return permissions.Any(p => principal.HasClaim(EnergyClaimTypes.Permission, p));
    }

    public static IReadOnlyList<Guid> GetRoleIds(this ClaimsPrincipal? principal)
    {
        if (principal is null)
        {
            return Array.Empty<Guid>();
        }

        return principal.FindAll(EnergyClaimTypes.RoleId)
            .Select(c => Guid.TryParse(c.Value, out var id) ? id : Guid.Empty)
            .Where(id => id != Guid.Empty)
            .ToArray();
    }

    /// <summary>
    /// Returns the user's stable role keys (role <c>NormalizedName</c> values).
    /// </summary>
    public static IReadOnlyList<string> GetRoleKeys(this ClaimsPrincipal? principal)
    {
        if (principal is null)
        {
            return Array.Empty<string>();
        }

        return principal.FindAll(EnergyClaimTypes.RoleKey)
            .Select(c => c.Value)
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .ToArray();
    }

    /// <summary>
    /// Checks the role-key claim (culture-independent <c>NormalizedName</c>)
    /// instead of the localized display name returned by
    /// <see cref="ClaimsPrincipal.IsInRole"/>.
    /// </summary>
    public static bool HasRoleKey(this ClaimsPrincipal? principal, string roleKey)
    {
        if (principal is null || string.IsNullOrWhiteSpace(roleKey))
        {
            return false;
        }

        return principal.FindAll(EnergyClaimTypes.RoleKey)
            .Any(c => string.Equals(c.Value, roleKey, StringComparison.OrdinalIgnoreCase));
    }

    public static string? GetFullName(this ClaimsPrincipal? principal)
    {
        return principal?.FindFirstValue(EnergyClaimTypes.FullName)
            ?? principal?.Identity?.Name;
    }

    public static Guid? GetUserId(this ClaimsPrincipal? principal)
    {
        var raw = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(raw, out var id) ? id : null;
    }
}

