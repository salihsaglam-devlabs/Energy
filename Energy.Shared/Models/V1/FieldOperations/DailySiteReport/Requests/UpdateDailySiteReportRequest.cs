namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Requests;

/// <summary>DailySiteReport güncelleme isteği.</summary>
public class UpdateDailySiteReportRequest
{
    /// <summary>Güncellenecek kaydın kimliği.</summary>
    public Guid Id { get; set; }

    /// <summary>Projects referansı</summary>
    public Guid ProjectId { get; set; }

    /// <summary>WorkOrderId</summary>
    public Guid? WorkOrderId { get; set; }

    /// <summary>ReportNo</summary>
    public string ReportNo { get; set; } = string.Empty;

    /// <summary>ReportDate</summary>
    public DateTime ReportDate { get; set; }

    /// <summary>Weather</summary>
    public string? Weather { get; set; }

    /// <summary>Notes</summary>
    public string? Notes { get; set; }

    /// <summary>Status</summary>
    public string Status { get; set; } = string.Empty;
}
