namespace Energy.Shared.Models.V1.Localization.Responses;

public sealed class LocalizationEntryResponse
{
    public string Key { get; init; } = string.Empty;
    public IReadOnlyDictionary<string, string> Values { get; init; } = new Dictionary<string, string>();
}
