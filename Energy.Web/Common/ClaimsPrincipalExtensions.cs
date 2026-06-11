using System.Security.Claims;
using Energy.Shared.Identity;

namespace Energy.Web.Common;

/// <summary>
/// Custom claim types written to the cookie principal by the auth pipeline.
/// Mirrors the JWT claims emitted by the API.
/// </summary>
public static class EnergyClaimTypes
{
    public const string Permission = "permission";
    public const string RoleId = "role_id";
    public const string RoleKey = "role_key";
    public const string FullName = "full_name";
}

public static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal? principal)
    {
        var raw = principal?.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(raw, out var id) ? id : null;
    }

    public static string? GetEmail(this ClaimsPrincipal? principal)
        => principal?.FindFirstValue(ClaimTypes.Email);

    public static string? GetUserName(this ClaimsPrincipal? principal)
        => principal?.Identity?.Name;

    public static string? GetDisplayName(this ClaimsPrincipal? principal)
        => principal?.FindFirstValue(EnergyClaimTypes.FullName)
           ?? principal?.FindFirstValue("display_name");

    public static string? GetFullName(this ClaimsPrincipal? principal)
        => principal.GetDisplayName() ?? principal?.Identity?.Name;

    public static bool IsSuperAdmin(this ClaimsPrincipal? principal)
        => principal is not null && principal.IsInRole(SystemRoles.SuperAdmin);

    public static bool HasPermission(this ClaimsPrincipal? principal, string permission)
    {
        if (principal is null || string.IsNullOrEmpty(permission)) return false;
        // SuperAdmin is unrestricted: no permission claim is ever required.
        if (principal.IsSuperAdmin()) return true;
        return principal.HasClaim(EnergyClaimTypes.Permission, permission);
    }

    public static IReadOnlyList<Guid> GetRoleIds(this ClaimsPrincipal? principal)
    {
        if (principal is null) return Array.Empty<Guid>();
        return principal.FindAll(EnergyClaimTypes.RoleId)
            .Select(c => Guid.TryParse(c.Value, out var id) ? id : Guid.Empty)
            .Where(id => id != Guid.Empty)
            .ToArray();
    }

    public static IReadOnlyList<string> GetRoleKeys(this ClaimsPrincipal? principal)
    {
        if (principal is null) return Array.Empty<string>();
        return principal.FindAll(EnergyClaimTypes.RoleKey)
            .Select(c => c.Value)
            .Where(v => !string.IsNullOrWhiteSpace(v))
            .ToArray();
    }

    public static bool HasRoleKey(this ClaimsPrincipal? principal, string roleKey)
    {
        if (principal is null || string.IsNullOrWhiteSpace(roleKey)) return false;
        return principal.FindAll(EnergyClaimTypes.RoleKey)
            .Any(c => string.Equals(c.Value, roleKey, StringComparison.OrdinalIgnoreCase));
    }
}
