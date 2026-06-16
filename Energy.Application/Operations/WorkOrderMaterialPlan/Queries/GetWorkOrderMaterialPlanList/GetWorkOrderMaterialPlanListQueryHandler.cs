using Energy.Application.Operations.WorkOrderMaterialPlan.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialPlan.Queries.GetWorkOrderMaterialPlanList;

/// <summary>
/// <see cref="GetWorkOrderMaterialPlanListQuery"/> handler'ı. <see cref="IWorkOrderMaterialPlanService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderMaterialPlanListQueryHandler
    : IRequestHandler<GetWorkOrderMaterialPlanListQuery, BaseResponse<PaginatedResponse<WorkOrderMaterialPlanListResponse>>>
{
    private readonly IWorkOrderMaterialPlanService _service;

    public GetWorkOrderMaterialPlanListQueryHandler(IWorkOrderMaterialPlanService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<WorkOrderMaterialPlanListResponse>>> Handle(
        GetWorkOrderMaterialPlanListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
