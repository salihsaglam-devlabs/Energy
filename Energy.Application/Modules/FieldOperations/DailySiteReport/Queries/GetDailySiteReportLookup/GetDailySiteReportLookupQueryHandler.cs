using Energy.Application.Modules.FieldOperations.DailySiteReport.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReport.Queries.GetDailySiteReportLookup;

/// <summary>
/// <see cref="GetDailySiteReportLookupQuery"/> handler'ı. <see cref="IDailySiteReportLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportLookupQueryHandler
    : IRequestHandler<GetDailySiteReportLookupQuery, BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>>
{
    private readonly IDailySiteReportLookupService _lookup;

    public GetDailySiteReportLookupQueryHandler(IDailySiteReportLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DailySiteReportLookupResponse>>> Handle(
        GetDailySiteReportLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
