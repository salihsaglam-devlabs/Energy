using Energy.Domain.Common;

namespace Energy.Domain.Modules.Assets;

/// <summary>Ekipman ve demirbaş kartı.</summary>
public class EquipmentAsset : AuditableEntity
{
    public Guid CompanyId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    /// <summary>Vehicle, Machine, Device, Tool, Fixture.</summary>
    public string AssetType { get; set; } = "Machine";
    public string? SerialNo { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public bool IsActive { get; set; } = true;
}
