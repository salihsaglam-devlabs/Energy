using Energy.Application.Operations.WorkOrderChecklistItem.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklistItem.Queries.GetWorkOrderChecklistItemLookup;

/// <summary>
/// <see cref="GetWorkOrderChecklistItemLookupQuery"/> handler'ı. <see cref="IWorkOrderChecklistItemLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderChecklistItemLookupQueryHandler
    : IRequestHandler<GetWorkOrderChecklistItemLookupQuery, BaseResponse<IReadOnlyList<WorkOrderChecklistItemLookupResponse>>>
{
    private readonly IWorkOrderChecklistItemLookupService _lookup;

    public GetWorkOrderChecklistItemLookupQueryHandler(IWorkOrderChecklistItemLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WorkOrderChecklistItemLookupResponse>>> Handle(
        GetWorkOrderChecklistItemLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
