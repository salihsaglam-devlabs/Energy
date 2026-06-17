using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;

/// <summary>DailySiteReport liste satırı.</summary>
public class DailySiteReportListResponse
{
    /// <summary>Kimlik.</summary>
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
    public DocumentStatus Status { get; set; }

    /// <summary>Oluşturma zamanı.</summary>
    public DateTime CreatedAt { get; set; }
}
