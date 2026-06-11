namespace Energy.Shared.Identity.Permissions;

public static class AccessRulePermissions
{
    public const string GetAccessRules = "AccessRule.GetAccessRules";
    public const string GetAccessRule = "AccessRule.GetAccessRule";
    public const string CreateAccessRule = "AccessRule.CreateAccessRule";
    public const string UpdateAccessRule = "AccessRule.UpdateAccessRule";
    public const string DeleteAccessRule = "AccessRule.DeleteAccessRule";
    public const string GetAccessRulePermissions = "AccessRule.GetAccessRulePermissions";
    public const string SetAccessRulePermissions = "AccessRule.SetAccessRulePermissions";
    public const string GetRequiredPermissions = "AccessRule.GetRequiredPermissions";

    public static IReadOnlyList<string> All { get; } = new[]
    {
        GetAccessRules,
        GetAccessRule,
        CreateAccessRule,
        UpdateAccessRule,
        DeleteAccessRule,
        GetAccessRulePermissions,
        SetAccessRulePermissions,
        GetRequiredPermissions,
    };
}
