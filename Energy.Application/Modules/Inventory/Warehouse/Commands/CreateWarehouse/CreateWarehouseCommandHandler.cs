using Energy.Application.Modules.Inventory.Warehouse.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.Warehouse.Commands.CreateWarehouse;

/// <summary>
/// <see cref="CreateWarehouseCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWarehouseService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWarehouseCommandHandler
    : IRequestHandler<CreateWarehouseCommand, BaseResponse<Guid>>
{
    private readonly IWarehouseService _service;

    public CreateWarehouseCommandHandler(IWarehouseService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWarehouseCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
