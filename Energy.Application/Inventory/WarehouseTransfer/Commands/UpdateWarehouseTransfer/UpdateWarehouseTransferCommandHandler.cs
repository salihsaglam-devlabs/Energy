using Energy.Application.Inventory.WarehouseTransfer.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransfer.Commands.UpdateWarehouseTransfer;

/// <summary>
/// <see cref="UpdateWarehouseTransferCommand"/> handler'ı. <see cref="IWarehouseTransferService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWarehouseTransferCommandHandler
    : IRequestHandler<UpdateWarehouseTransferCommand, BaseResponse<bool>>
{
    private readonly IWarehouseTransferService _service;

    public UpdateWarehouseTransferCommandHandler(IWarehouseTransferService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWarehouseTransferCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
