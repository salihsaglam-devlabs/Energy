namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReportMaterial.Responses;

/// <summary>DailySiteReportMaterial liste satırı.</summary>
public class DailySiteReportMaterialListResponse
{
    /// <summary>Kimlik.</summary>
    public Guid Id { get; set; }

    /// <summary>DailySiteReports referansı</summary>
    public Guid DailySiteReportId { get; set; }

    /// <summary>Materials referansı</summary>
    public Guid MaterialId { get; set; }

    /// <summary>Quantity</summary>
    public decimal Quantity { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
