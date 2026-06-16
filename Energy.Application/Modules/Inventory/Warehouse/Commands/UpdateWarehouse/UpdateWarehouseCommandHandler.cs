using Energy.Application.Modules.Inventory.Warehouse.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.Warehouse.Commands.UpdateWarehouse;

/// <summary>
/// <see cref="UpdateWarehouseCommand"/> handler'ı. <see cref="IWarehouseService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWarehouseCommandHandler
    : IRequestHandler<UpdateWarehouseCommand, BaseResponse<bool>>
{
    private readonly IWarehouseService _service;

    public UpdateWarehouseCommandHandler(IWarehouseService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWarehouseCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
