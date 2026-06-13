using Energy.Domain.Common;

namespace Energy.Domain.Assets;

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

/// <summary>Ekipman bakım kaydı (planlı/plansız).</summary>
public class EquipmentMaintenance : AuditableEntity
{
    public Guid EquipmentAssetId { get; set; }
    /// <summary>Planned, Unplanned.</summary>
    public string MaintenanceType { get; set; } = "Planned";
    public DateTime? ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public decimal Cost { get; set; }
    public string? Note { get; set; }
}

