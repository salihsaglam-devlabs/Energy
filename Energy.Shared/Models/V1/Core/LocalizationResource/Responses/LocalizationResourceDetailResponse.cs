namespace Energy.Shared.Models.V1.Core.LocalizationResource.Responses;

/// <summary>LocalizationResource detay görünümü.</summary>
public class LocalizationResourceDetailResponse
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

    /// <summary>Alternatif anahtar</summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>Culture</summary>
    public string Culture { get; set; } = string.Empty;

    /// <summary>Value</summary>
    public string Value { get; set; } = string.Empty;
}
