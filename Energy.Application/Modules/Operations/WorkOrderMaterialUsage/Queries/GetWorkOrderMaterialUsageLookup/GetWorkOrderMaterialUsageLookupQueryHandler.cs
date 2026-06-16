using Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Queries.GetWorkOrderMaterialUsageLookup;

/// <summary>
/// <see cref="GetWorkOrderMaterialUsageLookupQuery"/> handler'ı. <see cref="IWorkOrderMaterialUsageLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderMaterialUsageLookupQueryHandler
    : IRequestHandler<GetWorkOrderMaterialUsageLookupQuery, BaseResponse<IReadOnlyList<WorkOrderMaterialUsageLookupResponse>>>
{
    private readonly IWorkOrderMaterialUsageLookupService _lookup;

    public GetWorkOrderMaterialUsageLookupQueryHandler(IWorkOrderMaterialUsageLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WorkOrderMaterialUsageLookupResponse>>> Handle(
        GetWorkOrderMaterialUsageLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
