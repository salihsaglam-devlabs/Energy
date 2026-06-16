using Energy.Application.Operations.WorkOrderMaterialPlan.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialPlan.Commands.CreateWorkOrderMaterialPlan;

/// <summary>
/// <see cref="CreateWorkOrderMaterialPlanCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWorkOrderMaterialPlanService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWorkOrderMaterialPlanCommandHandler
    : IRequestHandler<CreateWorkOrderMaterialPlanCommand, BaseResponse<Guid>>
{
    private readonly IWorkOrderMaterialPlanService _service;

    public CreateWorkOrderMaterialPlanCommandHandler(IWorkOrderMaterialPlanService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWorkOrderMaterialPlanCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
