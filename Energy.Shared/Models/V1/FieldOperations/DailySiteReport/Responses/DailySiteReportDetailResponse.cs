using Energy.Shared.Common;
namespace Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;

/// <summary>DailySiteReport detay görünümü.</summary>
public class DailySiteReportDetailResponse
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
}
