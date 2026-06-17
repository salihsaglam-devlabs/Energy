namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Responses;

/// <summary>DailySiteReportEquipment liste satırı.</summary>
public class DailySiteReportEquipmentListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>DailySiteReportId</summary>
    public Guid DailySiteReportId { get; set; }

    /// <summary>EquipmentAssetId</summary>
    public Guid? EquipmentAssetId { get; set; }

    /// <summary>EquipmentText</summary>
    public string? EquipmentText { get; set; }

    /// <summary>Hours</summary>
    public decimal Hours { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
