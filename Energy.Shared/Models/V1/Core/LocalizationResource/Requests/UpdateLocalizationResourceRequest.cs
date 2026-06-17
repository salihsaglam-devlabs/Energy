namespace Energy.Shared.Models.V1.Core.LocalizationResource.Requests;

/// <summary>LocalizationResource güncelleme isteği.</summary>
public class UpdateLocalizationResourceRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Alternatif anahtar</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Culture</summary>
    public string Culture { get; set; } = string.Empty;

    /// <summary>Value</summary>
    public string Value { get; set; } = string.Empty;
}
