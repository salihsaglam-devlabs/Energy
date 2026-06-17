namespace Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Requests;

/// <summary>EquipmentMaintenance güncelleme isteği.</summary>
public class UpdateEquipmentMaintenanceRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>EquipmentAssetId</summary>
    public Guid EquipmentAssetId { get; set; }

    /// <summary>MaintenanceType</summary>
    public string MaintenanceType { get; set; } = string.Empty;

    /// <summary>ScheduledDate</summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>CompletedDate</summary>
    public DateTime? CompletedDate { get; set; }

    /// <summary>Cost</summary>
    public decimal Cost { get; set; }

    /// <summary>Note</summary>
    public string? Note { get; set; }
}
