namespace Energy.Shared.Identity;

/// <summary>
/// Stable, culture-independent identifiers for built-in roles. The seeded
/// admin role's <c>NormalizedName</c> always equals <see cref="Admin"/> so
/// the Web/Api layers can detect it reliably even when its display name is
/// localized (e.g. "Yönetici" in Turkish).
/// </summary>
public static class SystemRoleKeys
{
    public const string Admin = "ADMIN";
}

