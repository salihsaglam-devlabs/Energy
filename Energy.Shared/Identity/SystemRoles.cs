namespace Energy.Shared.Identity;

/// <summary>
/// Stable identifiers for built-in roles. The SuperAdmin role always exists
/// and bypasses every permission check; it cannot be renamed or deleted.
/// </summary>
public static class SystemRoles
{
    public const string SuperAdmin = "SuperAdmin";
}

