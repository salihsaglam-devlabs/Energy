namespace Energy.Shared.Identity.Permissions;

public static class LocalizationPermissions
{
    public const string GetAll = "Localization.GetAll";
    public const string GetByKey = "Localization.GetByKey";
    public const string Upsert = "Localization.Upsert";
    public const string Delete = "Localization.Delete";

    public static IReadOnlyList<string> All { get; } = new[]
    {
        GetAll,
        GetByKey,
        Upsert,
        Delete,
    };
}
