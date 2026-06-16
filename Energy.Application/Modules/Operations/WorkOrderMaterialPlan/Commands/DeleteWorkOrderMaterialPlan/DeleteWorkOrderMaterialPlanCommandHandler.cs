using Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Commands.DeleteWorkOrderMaterialPlan;

/// <summary>
/// <see cref="DeleteWorkOrderMaterialPlanCommand"/> handler'ı. <see cref="IWorkOrderMaterialPlanService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWorkOrderMaterialPlanCommandHandler
    : IRequestHandler<DeleteWorkOrderMaterialPlanCommand, BaseResponse<bool>>
{
    private readonly IWorkOrderMaterialPlanService _service;

    public DeleteWorkOrderMaterialPlanCommandHandler(IWorkOrderMaterialPlanService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWorkOrderMaterialPlanCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
