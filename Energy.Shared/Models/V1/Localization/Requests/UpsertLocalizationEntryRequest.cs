namespace Energy.Shared.Models.V1.Localization.Requests;

public sealed class UpsertLocalizationEntryRequest
{
    public string Key { get; set; } = string.Empty;

    /// <summary>Map of (culture → value). Use empty string for the invariant culture.</summary>
    public Dictionary<string, string> Values { get; set; } = new();
}
