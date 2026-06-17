using Energy.Application.Inventory.Warehouse.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.Warehouse.Commands.DeleteWarehouse;

/// <summary>
/// <see cref="DeleteWarehouseCommand"/> handler'ı. <see cref="IWarehouseService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWarehouseCommandHandler
    : IRequestHandler<DeleteWarehouseCommand, BaseResponse<bool>>
{
    private readonly IWarehouseService _service;

    public DeleteWarehouseCommandHandler(IWarehouseService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWarehouseCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
