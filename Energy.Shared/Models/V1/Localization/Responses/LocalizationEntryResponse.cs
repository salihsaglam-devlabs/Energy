namespace Energy.Shared.Models.V1.Localization.Responses;

public sealed class LocalizationEntryResponse
{
    public string Key { get; init; } = string.Empty;

    /// <summary>
    /// Map of culture → value for this key (e.g. {"tr-TR": "Kaydet", "en-US": "Save"}).
    /// </summary>
    public Dictionary<string, string> Values { get; init; } = new();
}

