namespace Energy.Shared.Models.V1.Localization.Responses;

/// <summary>Bir yerelleştirme anahtarı ve onun kültür bazlı değerleri.</summary>
public sealed class LocalizationEntryResponse
{
    /// <summary>Yerelleştirme anahtarı.</summary>
    public string Key { get; init; } = string.Empty;

    /// <summary>(Kültür → değer) eşlemesi.</summary>
    public IReadOnlyDictionary<string, string> Values { get; init; } = new Dictionary<string, string>();
}
