using Energy.Application.Modules.Reporting.ReportDefinition.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Responses;
using MediatR;

namespace Energy.Application.Modules.Reporting.ReportDefinition.Queries.GetReportDefinitionLookup;

/// <summary>
/// <see cref="GetReportDefinitionLookupQuery"/> handler'ı. <see cref="IReportDefinitionLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetReportDefinitionLookupQueryHandler
    : IRequestHandler<GetReportDefinitionLookupQuery, BaseResponse<IReadOnlyList<ReportDefinitionLookupResponse>>>
{
    private readonly IReportDefinitionLookupService _lookup;

    public GetReportDefinitionLookupQueryHandler(IReportDefinitionLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<ReportDefinitionLookupResponse>>> Handle(
        GetReportDefinitionLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
