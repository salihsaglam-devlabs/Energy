namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;

/// <summary>DailySiteReportMaterial güncelleme isteği.</summary>
public class UpdateDailySiteReportMaterialRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>DailySiteReports referansı</summary>
    public Guid DailySiteReportId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }
}
