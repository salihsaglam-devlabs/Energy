using Energy.Domain.Common;

namespace Energy.Domain.Modules.Assets;

/// <summary>
/// Ekipman atamaları
/// </summary>
public class EquipmentAssignment : AuditableEntity
{
    /// <summary>EquipmentAssetId</summary>
    public Guid EquipmentAssetId { get; set; }

    /// <summary>ProjectId</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>EmployeeId</summary>
    public Guid? EmployeeId { get; set; }

    /// <summary>WarehouseId</summary>
    public Guid? WarehouseId { get; set; }

    /// <summary>StartDate</summary>
    public DateTime StartDate { get; set; }

    /// <summary>EndDate</summary>
    public DateTime? EndDate { get; set; }

    /// <summary>IsActive</summary>
    public bool IsActive { get; set; }
}
