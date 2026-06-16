namespace Energy.Shared.Models.V1.Core.LocalizationResource.Requests;

/// <summary>LocalizationResource oluşturma isteği.</summary>
public class CreateLocalizationResourceRequest
{
    /// <summary>Alternatif anahtar</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Culture</summary>
    public string Culture { get; set; } = string.Empty;

    /// <summary>Value</summary>
    public string Value { get; set; } = string.Empty;
}
