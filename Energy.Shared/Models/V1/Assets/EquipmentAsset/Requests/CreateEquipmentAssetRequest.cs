namespace Energy.Shared.Models.V1.Assets.EquipmentAsset.Requests;

/// <summary>EquipmentAsset oluşturma isteği.</summary>
public class CreateEquipmentAssetRequest
{
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
