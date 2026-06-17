using Energy.Application.Inventory.WarehouseLocation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseLocation.Commands.DeleteWarehouseLocation;

/// <summary>
/// <see cref="DeleteWarehouseLocationCommand"/> handler'ı. <see cref="IWarehouseLocationService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWarehouseLocationCommandHandler
    : IRequestHandler<DeleteWarehouseLocationCommand, BaseResponse<bool>>
{
    private readonly IWarehouseLocationService _service;

    public DeleteWarehouseLocationCommandHandler(IWarehouseLocationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWarehouseLocationCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
