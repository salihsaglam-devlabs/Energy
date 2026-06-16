using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Requests;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Responses;

namespace Energy.Application.Projects.Reports.ProjectStatusReport.Services;

/// <summary>ProjectStatusReport raporu servis sözleşmesi (salt-okunur).</summary>
public interface IProjectStatusReportService
{
    /// <summary>Filtrelenmiş, sayfalanmış rapor verisini döndürür.</summary>
    Task<BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>> GetDataAsync(ProjectStatusReportRequest request, CancellationToken ct = default);
}
