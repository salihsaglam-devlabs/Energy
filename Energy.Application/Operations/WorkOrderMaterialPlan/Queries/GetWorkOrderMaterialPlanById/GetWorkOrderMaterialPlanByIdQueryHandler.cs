using Energy.Application.Operations.WorkOrderMaterialPlan.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialPlan.Queries.GetWorkOrderMaterialPlanById;

/// <summary>
/// <see cref="GetWorkOrderMaterialPlanByIdQuery"/> handler'ı. <see cref="IWorkOrderMaterialPlanService"/>'i orkestre eder.
/// </summary>
public sealed class GetWorkOrderMaterialPlanByIdQueryHandler
    : IRequestHandler<GetWorkOrderMaterialPlanByIdQuery, BaseResponse<WorkOrderMaterialPlanDetailResponse>>
{
    private readonly IWorkOrderMaterialPlanService _service;

    public GetWorkOrderMaterialPlanByIdQueryHandler(IWorkOrderMaterialPlanService service)
        => _service = service;

    public Task<BaseResponse<WorkOrderMaterialPlanDetailResponse>> Handle(
        GetWorkOrderMaterialPlanByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
