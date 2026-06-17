namespace Energy.Shared.Models.V1.Core.SystemSetting.Requests;

/// <summary>SystemSetting güncelleme isteği.</summary>
public class UpdateSystemSettingRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Key</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Value</summary>
    public string? Value { get; set; }

    /// <summary>Category</summary>
    public string? Category { get; set; }

    /// <summary>DescriptionKey</summary>
    public string? DescriptionKey { get; set; }
}
