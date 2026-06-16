using Energy.Application.Modules.Operations.WorkOrderType.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderType.Queries.GetWorkOrderTypeLookup;

/// <summary>
/// <see cref="GetWorkOrderTypeLookupQuery"/> handler'ı. <see cref="IWorkOrderTypeLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderTypeLookupQueryHandler
    : IRequestHandler<GetWorkOrderTypeLookupQuery, BaseResponse<IReadOnlyList<WorkOrderTypeLookupResponse>>>
{
    private readonly IWorkOrderTypeLookupService _lookup;

    public GetWorkOrderTypeLookupQueryHandler(IWorkOrderTypeLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WorkOrderTypeLookupResponse>>> Handle(
        GetWorkOrderTypeLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
