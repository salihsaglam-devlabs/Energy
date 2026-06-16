namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReportEquipment.Requests;

/// <summary>DailySiteReportEquipment güncelleme isteği.</summary>
public class UpdateDailySiteReportEquipmentRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>DailySiteReportId</summary>
    public Guid DailySiteReportId { get; set; }

    /// <summary>EquipmentAssetId</summary>
    public Guid? EquipmentAssetId { get; set; }

    /// <summary>EquipmentText</summary>
    public string? EquipmentText { get; set; }

    /// <summary>Hours</summary>
    public decimal Hours { get; set; }
}
