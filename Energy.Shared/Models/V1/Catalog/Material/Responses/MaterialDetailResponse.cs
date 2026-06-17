namespace Energy.Shared.Models.V1.Catalog.Material.Responses;

/// <summary>Material detay görünümü.</summary>
public class MaterialDetailResponse
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

    /// <summary>Kategori</summary>
    public Guid MaterialCategoryId { get; set; }

    /// <summary>Marka</summary>
    public Guid? BrandId { get; set; }

    /// <summary>Temel birim</summary>
    public Guid BaseUnitOfMeasureId { get; set; }

    /// <summary>Malzeme kodu</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Malzeme adı</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Lot takibi</summary>
    public bool IsBatchTracked { get; set; }

    /// <summary>Seri takibi</summary>
    public bool IsSerialTracked { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
