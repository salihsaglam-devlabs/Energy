using Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Commands.UpdateWorkOrderMaterialPlan;

/// <summary>
/// <see cref="UpdateWorkOrderMaterialPlanCommand"/> handler'ı. <see cref="IWorkOrderMaterialPlanService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWorkOrderMaterialPlanCommandHandler
    : IRequestHandler<UpdateWorkOrderMaterialPlanCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderMaterialPlanService _service;

    public UpdateWorkOrderMaterialPlanCommandHandler(IWorkOrderMaterialPlanService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWorkOrderMaterialPlanCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
