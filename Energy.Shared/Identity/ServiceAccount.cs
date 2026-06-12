namespace Energy.Shared.Identity;

/// <summary>
/// Built-in, non-interactive system/service account used for system-to-system
/// calls — e.g. Web-tier audit ingestion for anonymous/internal requests that
/// have no signed-in user. It is assigned the <see cref="SystemRoles.SuperAdmin"/>
/// role so it bypasses every permission check, and it is completely independent
/// from any human (authenticated) user.
///
/// The password is read from configuration ("ServiceAccount:Password" on the API
/// side, "Api:ServiceAccount:Password" on the Web side) and falls back to
/// <see cref="DefaultPassword"/> so both tiers agree out of the box.
/// </summary>
public static class ServiceAccount
{
    public const string UserName = "system";
    public const string Email = "system@energy.local";
    public const string FirstName = "System";
    public const string LastName = "Service";

    /// <summary>Fallback secret used when no override is configured. Override it
    /// in production via configuration on BOTH the API and the Web tier.</summary>
    public const string DefaultPassword = "Sys!Service#2024$Energy";

    /// <summary>Configuration key the API reads to override the seeded password.</summary>
    public const string ApiPasswordConfigKey = "ServiceAccount:Password";

    /// <summary>Configuration keys the Web tier reads to override the credentials.</summary>
    public const string WebUserNameConfigKey = "Api:ServiceAccount:UserNameOrEmail";
    public const string WebPasswordConfigKey = "Api:ServiceAccount:Password";
}

