namespace Energy.Shared.Models.V1.Localization.Requests;

/// <summary>
/// Insert-or-update payload for a single localization key. The dictionary
/// maps culture name (e.g. "tr-TR", "en-US", or "" for invariant) to value.
/// Cultures absent from the payload are left untouched.
/// </summary>
public sealed class UpsertLocalizationEntryRequest
{
    public string Key { get; init; } = string.Empty;

    public Dictionary<string, string> Values { get; init; } = new();
}

