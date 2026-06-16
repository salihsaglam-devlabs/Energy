using Energy.Domain.Common;

namespace Energy.Domain.Modules.Assets;

/// <summary>Ekipman ataması (proje, çalışan, depo veya lokasyon).</summary>
public class EquipmentAssignment : AuditableEntity
{
    public Guid EquipmentAssetId { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? WarehouseId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; } = true;
}
