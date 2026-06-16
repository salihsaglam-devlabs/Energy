namespace Energy.Shared.Models.V1.IAM.Menu.Responses;

/// <summary>Menu detay görünümü.</summary>
public class MenuDetailResponse
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

    /// <summary>Üst menü</summary>
    public Guid? ParentId { get; set; }

    /// <summary>Lokalizasyon anahtarı</summary>
    public string NameKey { get; set; } = string.Empty;

    /// <summary>URL</summary>
    public string? Url { get; set; }

    /// <summary>Gerekli permission</summary>
    public string? RequiredPermissionCode { get; set; }
}
