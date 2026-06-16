using Energy.Application.Modules.Finance.CostCenter.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.CostCenter.Commands.DeleteCostCenter;

/// <summary>
/// <see cref="DeleteCostCenterCommand"/> handler'ı. <see cref="ICostCenterService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteCostCenterCommandHandler
    : IRequestHandler<DeleteCostCenterCommand, BaseResponse<bool>>
{
    private readonly ICostCenterService _service;

    public DeleteCostCenterCommandHandler(ICostCenterService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteCostCenterCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
