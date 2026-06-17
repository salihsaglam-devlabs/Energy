namespace Energy.Shared.Models.V1.Core.LocalizationResource.Responses;

/// <summary>LocalizationResource liste satırı.</summary>
public class LocalizationResourceListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Alternatif anahtar</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Culture</summary>
    public string Culture { get; set; } = string.Empty;

    /// <summary>Value</summary>
    public string Value { get; set; } = string.Empty;

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
