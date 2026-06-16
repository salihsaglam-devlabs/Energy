using Energy.Domain.Common;

namespace Energy.Domain.Modules.FieldOperations;

/// <summary>
/// Günlük saha ekipmanları
/// </summary>
public class DailySiteReportEquipment : AuditableEntity
{
    /// <summary>DailySiteReportId</summary>
    public Guid DailySiteReportId { get; set; }

    /// <summary>EquipmentAssetId</summary>
    public Guid? EquipmentAssetId { get; set; }

    /// <summary>EquipmentText</summary>
    public string? EquipmentText { get; set; }

    /// <summary>Hours</summary>
    public decimal Hours { get; set; }
}
