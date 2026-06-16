using Energy.Application.Finance.CostCenter.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Finance.CostCenter.Commands.UpdateCostCenter;

/// <summary>
/// <see cref="UpdateCostCenterCommand"/> handler'ı. <see cref="ICostCenterService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateCostCenterCommandHandler
    : IRequestHandler<UpdateCostCenterCommand, BaseResponse<bool>>
{
    private readonly ICostCenterService _service;

    public UpdateCostCenterCommandHandler(ICostCenterService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateCostCenterCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
