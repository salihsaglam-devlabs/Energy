using Energy.Domain.Common;

namespace Energy.Domain.Assets;

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
