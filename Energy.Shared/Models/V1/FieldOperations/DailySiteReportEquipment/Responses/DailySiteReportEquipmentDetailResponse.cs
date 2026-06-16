namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;

/// <summary>DailySiteReportEquipment detay görünümü.</summary>
public class DailySiteReportEquipmentDetailResponse
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

    /// <summary>DailySiteReportId</summary>
    public Guid DailySiteReportId { get; set; }

    /// <summary>EquipmentAssetId</summary>
    public Guid? EquipmentAssetId { get; set; }

    /// <summary>EquipmentText</summary>
    public string? EquipmentText { get; set; }

    /// <summary>Hours</summary>
    public decimal Hours { get; set; }
}
