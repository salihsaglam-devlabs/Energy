using Energy.Application.Reporting.ReportDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;
using MediatR;

namespace Energy.Application.Reporting.ReportDefinition.Queries.GetReportDefinitionList;

/// <summary>
/// <see cref="GetReportDefinitionListQuery"/> handler'ı. <see cref="IReportDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class GetReportDefinitionListQueryHandler
    : IRequestHandler<GetReportDefinitionListQuery, BaseResponse<PaginatedResponse<ReportDefinitionListResponse>>>
{
    private readonly IReportDefinitionService _service;

    public GetReportDefinitionListQueryHandler(IReportDefinitionService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<ReportDefinitionListResponse>>> Handle(
        GetReportDefinitionListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
