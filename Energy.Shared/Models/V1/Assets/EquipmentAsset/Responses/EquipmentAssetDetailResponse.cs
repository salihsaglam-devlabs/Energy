namespace Energy.Shared.Models.V1.Assets.EquipmentAsset.Responses;

/// <summary>EquipmentAsset detay görünümü.</summary>
public class EquipmentAssetDetailResponse
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

    /// <summary>CompanyId</summary>
    public Guid CompanyId { get; set; }

    /// <summary>Code</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Name</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>AssetType</summary>
    public string AssetType { get; set; } = string.Empty;

    /// <summary>SerialNo</summary>
    public string? SerialNo { get; set; }

    /// <summary>PurchaseDate</summary>
    public DateTime? PurchaseDate { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
