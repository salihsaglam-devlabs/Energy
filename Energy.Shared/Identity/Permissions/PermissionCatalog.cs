using System.Text;

namespace Energy.Shared.Identity.Permissions;

/// <summary>
/// Aggregates every permission code declared by the feature-specific permission classes
/// and derives the localization key + human readable fallback name from the code itself,
/// removing the need to repeat that metadata on every entry.
/// </summary>
public static class PermissionCatalog
{
    /// <summary>
    /// Flat list of every permission code declared in the application.
    /// </summary>
    public static IReadOnlyList<string> AllCodes { get; } =
        HomePermissions.All
            .Concat(UserPermissions.All)
            .Concat(PermissionPermissions.All)
            .Concat(RolePermissions.All)
            .Concat(MenuPermissions.All)
            .Concat(LocalizationPermissions.All)
            .Concat(AccessRulePermissions.All)
            .ToArray();

    /// <summary>
    /// Permission codes paired with the localization key and a human readable fallback
    /// name, both derived from the code itself.
    /// </summary>
    public static IReadOnlyList<PermissionDescriptor> All { get; } =
        AllCodes.Select(code => new PermissionDescriptor(code, BuildNameKey(code), BuildFallbackName(code))).ToArray();

    /// <summary>
    /// Builds the localization key for the given permission code.
    /// Example: <c>"Localization.GetAll"</c> -&gt; <c>"Permissions.Localization.GetAll.Name"</c>.
    /// </summary>
    public static string BuildNameKey(string code) => $"Permissions.{code}.Name";

    /// <summary>
    /// Builds a human readable fallback name for the given permission code by splitting
    /// the code on dots and PascalCase boundaries.
    /// Example: <c>"AccessRule.GetAccessRules"</c> -&gt; <c>"Access Rule Get Access Rules"</c>.
    /// </summary>
    public static string BuildFallbackName(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return string.Empty;
        }

        var segments = code.Split('.', StringSplitOptions.RemoveEmptyEntries);
        var parts = segments.Select(SplitPascalCase);
        return string.Join(' ', parts);
    }

    private static string SplitPascalCase(string value)
    {
        var builder = new StringBuilder(value.Length + 4);
        for (var i = 0; i < value.Length; i++)
        {
            var current = value[i];
            if (i > 0 && char.IsUpper(current) && !char.IsUpper(value[i - 1]))
            {
                builder.Append(' ');
            }

            builder.Append(current);
        }

        return builder.ToString();
    }
}

public readonly record struct PermissionDescriptor(string Code, string NameKey, string FallbackName);

