namespace Energy.Shared.Models.V1.Core.SystemSetting.Responses;

/// <summary>SystemSetting liste satırı.</summary>
public class SystemSettingListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Key</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Value</summary>
    public string? Value { get; set; }

    /// <summary>Category</summary>
    public string? Category { get; set; }

    /// <summary>DescriptionKey</summary>
    public string? DescriptionKey { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
