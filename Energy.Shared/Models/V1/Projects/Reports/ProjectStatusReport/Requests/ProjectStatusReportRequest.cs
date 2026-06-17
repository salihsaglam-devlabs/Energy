namespace Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Requests;

/// <summary>ProjectStatusReport raporu filtre/sayfalama isteği (salt-okunur).</summary>
public sealed class ProjectStatusReportRequest
{
    /// <summary>Sayfa numarası (1 tabanlı).</summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>Sayfa boyutu.</summary>
    public int PageSize { get; set; } = 50;

    /// <summary>Başlangıç tarihi filtresi (dahil).</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>Bitiş tarihi filtresi (dahil).</summary>
    public DateTime? EndDate { get; set; }
}
