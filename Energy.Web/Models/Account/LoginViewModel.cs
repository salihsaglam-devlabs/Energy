namespace Energy.Web.Models.Account;

public sealed class LoginViewModel
{
    public string? ReturnUrl { get; init; }

    /// <summary>
    /// Quick-login presets shown only in the Development environment so the
    /// known seed accounts can be selected without retyping credentials.
    /// Empty in every non-development environment.
    /// </summary>
    public IReadOnlyList<DevAccount> DevAccounts { get; init; } = Array.Empty<DevAccount>();
}

/// <summary>A seeded demo account exposed for one-click dev sign-in.</summary>
public sealed record DevAccount(string Label, string UserName, string Password);

/// <summary>
/// Catalog of seeded demo accounts (kept in sync with the infrastructure
/// <c>SystemSeeder</c>). Surfaced on the login page only in Development.
/// </summary>
public static class DevLoginAccounts
{
    public static readonly IReadOnlyList<DevAccount> All =
    [
        new("Admin — SuperAdmin", "admin", "Admin123!"),
        new("System Admin", "system.admin", "SysAdmin123!"),
        new("Operations Manager", "ops.manager", "OpsMgr123!"),
        new("Security Auditor", "security.auditor", "Auditor123!"),
        new("Localization Editor", "localization.editor", "Editor123!"),
        new("Read-only Viewer", "readonly.viewer", "Viewer123!"),
        new("Basic User", "basic.user", "Basic123!"),
    ];
}

