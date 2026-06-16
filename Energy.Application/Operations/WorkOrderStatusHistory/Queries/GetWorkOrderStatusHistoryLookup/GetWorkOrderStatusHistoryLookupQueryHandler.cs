using Energy.Application.Operations.WorkOrderStatusHistory.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderStatusHistory.Queries.GetWorkOrderStatusHistoryLookup;

/// <summary>
/// <see cref="GetWorkOrderStatusHistoryLookupQuery"/> handler'ı. <see cref="IWorkOrderStatusHistoryLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderStatusHistoryLookupQueryHandler
    : IRequestHandler<GetWorkOrderStatusHistoryLookupQuery, BaseResponse<IReadOnlyList<WorkOrderStatusHistoryLookupResponse>>>
{
    private readonly IWorkOrderStatusHistoryLookupService _lookup;

    public GetWorkOrderStatusHistoryLookupQueryHandler(IWorkOrderStatusHistoryLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WorkOrderStatusHistoryLookupResponse>>> Handle(
        GetWorkOrderStatusHistoryLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
