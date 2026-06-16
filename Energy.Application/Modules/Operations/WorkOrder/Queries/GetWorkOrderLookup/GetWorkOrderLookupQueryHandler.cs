using Energy.Application.Modules.Operations.WorkOrder.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrder.Queries.GetWorkOrderLookup;

/// <summary>
/// <see cref="GetWorkOrderLookupQuery"/> handler'ı. <see cref="IWorkOrderLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderLookupQueryHandler
    : IRequestHandler<GetWorkOrderLookupQuery, BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>>
{
    private readonly IWorkOrderLookupService _lookup;

    public GetWorkOrderLookupQueryHandler(IWorkOrderLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WorkOrderLookupResponse>>> Handle(
        GetWorkOrderLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
