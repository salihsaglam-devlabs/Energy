using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Requests;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Responses;
using MediatR;

namespace Energy.Application.Projects.Reports.ProjectStatusReport.Queries.GetProjectStatusReportData;

/// <summary>ProjectStatusReport rapor verisi (filtreli, sayfalı).</summary>
public sealed record GetProjectStatusReportDataQuery(ProjectStatusReportRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>>;
