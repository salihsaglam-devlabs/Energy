using Energy.Application.Modules.Operations.WorkOrderChecklist.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklist.Queries.GetWorkOrderChecklistLookup;

/// <summary>
/// <see cref="GetWorkOrderChecklistLookupQuery"/> handler'ı. <see cref="IWorkOrderChecklistLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderChecklistLookupQueryHandler
    : IRequestHandler<GetWorkOrderChecklistLookupQuery, BaseResponse<IReadOnlyList<WorkOrderChecklistLookupResponse>>>
{
    private readonly IWorkOrderChecklistLookupService _lookup;

    public GetWorkOrderChecklistLookupQueryHandler(IWorkOrderChecklistLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WorkOrderChecklistLookupResponse>>> Handle(
        GetWorkOrderChecklistLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
