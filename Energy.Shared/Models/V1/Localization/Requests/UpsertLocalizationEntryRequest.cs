namespace Energy.Shared.Models.V1.Localization.Requests;

/// <summary>Bir yerelleştirme anahtarını ve kültür değerlerini ekler veya günceller (upsert).</summary>
public sealed class UpsertLocalizationEntryRequest
{
    /// <summary>Yerelleştirme anahtarı.</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>(Kültür → değer) eşlemesi. Sabit (invariant) kültür için boş dize kullanın.</summary>
    public Dictionary<string, string> Values { get; set; } = new();
}
