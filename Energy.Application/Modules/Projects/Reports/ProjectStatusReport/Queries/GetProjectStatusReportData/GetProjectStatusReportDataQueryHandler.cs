using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Requests;
using Energy.Shared.Models.V1.Projects.Reports.ProjectStatusReport.Responses;
using Energy.Application.Modules.Projects.Reports.ProjectStatusReport.Services;
using MediatR;

namespace Energy.Application.Modules.Projects.Reports.ProjectStatusReport.Queries.GetProjectStatusReportData;

/// <summary><see cref="GetProjectStatusReportDataQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetProjectStatusReportDataQueryHandler
    : IRequestHandler<GetProjectStatusReportDataQuery, BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>>
{
    private readonly IProjectStatusReportService _service;

    public GetProjectStatusReportDataQueryHandler(IProjectStatusReportService service)
    {
        _service = service;
    }

    public async Task<BaseResponse<PaginatedResponse<ProjectStatusReportRowResponse>>> Handle(GetProjectStatusReportDataQuery request, CancellationToken ct)
    {
        return await _service.GetDataAsync(request.Request, ct);
    }
}
