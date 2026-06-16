using Energy.Application.Inventory.WarehouseTransfer.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransfer.Commands.CreateWarehouseTransfer;

/// <summary>
/// <see cref="CreateWarehouseTransferCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWarehouseTransferService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWarehouseTransferCommandHandler
    : IRequestHandler<CreateWarehouseTransferCommand, BaseResponse<Guid>>
{
    private readonly IWarehouseTransferService _service;

    public CreateWarehouseTransferCommandHandler(IWarehouseTransferService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWarehouseTransferCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
