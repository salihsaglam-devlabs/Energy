using Energy.Application.Reporting.ReportDefinition.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;
using MediatR;

namespace Energy.Application.Reporting.ReportDefinition.Queries.GetReportDefinitionById;

/// <summary>
/// <see cref="GetReportDefinitionByIdQuery"/> handler'ı. <see cref="IReportDefinitionService"/>'i orkestre eder.
/// </summary>
public sealed class GetReportDefinitionByIdQueryHandler
    : IRequestHandler<GetReportDefinitionByIdQuery, BaseResponse<ReportDefinitionDetailResponse>>
{
    private readonly IReportDefinitionService _service;

    public GetReportDefinitionByIdQueryHandler(IReportDefinitionService service)
        => _service = service;

    public Task<BaseResponse<ReportDefinitionDetailResponse>> Handle(
        GetReportDefinitionByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
