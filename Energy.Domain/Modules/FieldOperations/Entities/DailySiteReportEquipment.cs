using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>Günlük saha ekipmanı.</summary>
public class DailySiteReportEquipment : AuditableEntity
{
    public Guid DailySiteReportId { get; set; }
    public Guid? EquipmentAssetId { get; set; }
    public string? EquipmentText { get; set; }
    public decimal Hours { get; set; }
}
