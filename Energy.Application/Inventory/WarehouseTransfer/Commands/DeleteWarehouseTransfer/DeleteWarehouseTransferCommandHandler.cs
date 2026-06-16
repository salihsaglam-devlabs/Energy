using Energy.Application.Inventory.WarehouseTransfer.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransfer.Commands.DeleteWarehouseTransfer;

/// <summary>
/// <see cref="DeleteWarehouseTransferCommand"/> handler'ı. <see cref="IWarehouseTransferService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWarehouseTransferCommandHandler
    : IRequestHandler<DeleteWarehouseTransferCommand, BaseResponse<bool>>
{
    private readonly IWarehouseTransferService _service;

    public DeleteWarehouseTransferCommandHandler(IWarehouseTransferService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWarehouseTransferCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
