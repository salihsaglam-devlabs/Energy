using Energy.Application.Modules.Finance.CostCenter.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Finance.CostCenter.Commands.CreateCostCenter;

/// <summary>
/// <see cref="CreateCostCenterCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="ICostCenterService"/>'i orkestre eder.
/// </summary>
public sealed class CreateCostCenterCommandHandler
    : IRequestHandler<CreateCostCenterCommand, BaseResponse<Guid>>
{
    private readonly ICostCenterService _service;

    public CreateCostCenterCommandHandler(ICostCenterService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateCostCenterCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
