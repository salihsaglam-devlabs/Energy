using Energy.Application.Inventory.WarehouseLocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseLocation.Commands.CreateWarehouseLocation;

/// <summary>
/// <see cref="CreateWarehouseLocationCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWarehouseLocationService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWarehouseLocationCommandHandler
    : IRequestHandler<CreateWarehouseLocationCommand, BaseResponse<Guid>>
{
    private readonly IWarehouseLocationService _service;

    public CreateWarehouseLocationCommandHandler(IWarehouseLocationService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWarehouseLocationCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
