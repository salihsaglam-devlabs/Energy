namespace Energy.Shared.Models.V1.Catalog.Material.Requests;

/// <summary>Material güncelleme isteği.</summary>
public class UpdateMaterialRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

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
