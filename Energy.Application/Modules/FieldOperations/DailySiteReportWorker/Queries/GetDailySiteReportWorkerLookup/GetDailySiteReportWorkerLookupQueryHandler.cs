using Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Responses;
using MediatR;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Queries.GetDailySiteReportWorkerLookup;

/// <summary>
/// <see cref="GetDailySiteReportWorkerLookupQuery"/> handler'ı. <see cref="IDailySiteReportWorkerLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetDailySiteReportWorkerLookupQueryHandler
    : IRequestHandler<GetDailySiteReportWorkerLookupQuery, BaseResponse<IReadOnlyList<DailySiteReportWorkerLookupResponse>>>
{
    private readonly IDailySiteReportWorkerLookupService _lookup;

    public GetDailySiteReportWorkerLookupQueryHandler(IDailySiteReportWorkerLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<DailySiteReportWorkerLookupResponse>>> Handle(
        GetDailySiteReportWorkerLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
