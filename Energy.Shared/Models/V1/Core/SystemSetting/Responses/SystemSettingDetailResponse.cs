namespace Energy.Shared.Models.V1.Core.SystemSetting.Responses;

/// <summary>SystemSetting detay görünümü.</summary>
public class SystemSettingDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

    /// <summary>Key</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Value</summary>
    public string? Value { get; set; }

    /// <summary>Category</summary>
    public string? Category { get; set; }

    /// <summary>DescriptionKey</summary>
    public string? DescriptionKey { get; set; }
}
