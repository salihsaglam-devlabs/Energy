namespace Energy.Shared.Models.V1.Assets.EquipmentMaintenance.Responses;

/// <summary>EquipmentMaintenance detay görünümü.</summary>
public class EquipmentMaintenanceDetailResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>Oluşturma zamanı</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Oluşturan kullanıcı</summary>
    public Guid? CreatedBy { get; set; }

    /// <summary>Son güncelleme zamanı</summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>Güncelleyen kullanıcı</summary>
    public Guid? UpdatedBy { get; set; }

    /// <summary>Soft delete bayrağı</summary>
    public bool IsDeleted { get; set; }

    /// <summary>Silinme zamanı</summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>Silen kullanıcı</summary>
    public Guid? DeletedBy { get; set; }

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
