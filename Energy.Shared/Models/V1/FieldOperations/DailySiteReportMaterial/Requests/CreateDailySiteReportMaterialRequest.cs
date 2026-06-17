namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Requests;

/// <summary>DailySiteReportMaterial oluşturma isteği.</summary>
public class CreateDailySiteReportMaterialRequest
{
    /// <summary>DailySiteReports referansı</summary>
    public Guid DailySiteReportId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }
}
