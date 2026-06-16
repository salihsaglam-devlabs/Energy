using Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Lookups;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Queries.GetWorkOrderMaterialPlanLookup;

/// <summary>
/// <see cref="GetWorkOrderMaterialPlanLookupQuery"/> handler'ı. <see cref="IWorkOrderMaterialPlanLookupService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderMaterialPlanLookupQueryHandler
    : IRequestHandler<GetWorkOrderMaterialPlanLookupQuery, BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>>
{
    private readonly IWorkOrderMaterialPlanLookupService _lookup;

    public GetWorkOrderMaterialPlanLookupQueryHandler(IWorkOrderMaterialPlanLookupService lookup)
        => _lookup = lookup;

    public Task<BaseResponse<IReadOnlyList<WorkOrderMaterialPlanLookupResponse>>> Handle(
        GetWorkOrderMaterialPlanLookupQuery request,
        CancellationToken cancellationToken)
        => _lookup.GetLookupAsync(request.Search, request.ActiveOnly, cancellationToken);
}
