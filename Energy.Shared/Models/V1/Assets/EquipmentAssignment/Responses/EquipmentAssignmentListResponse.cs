namespace Energy.Shared.Models.V1.Assets.EquipmentAssignment.Responses;

/// <summary>EquipmentAssignment liste satırı.</summary>
public class EquipmentAssignmentListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

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

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
