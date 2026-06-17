using Energy.Application.Inventory.WarehouseTransferLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransferLine.Commands.CreateWarehouseTransferLine;

/// <summary>
/// <see cref="CreateWarehouseTransferLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IWarehouseTransferLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateWarehouseTransferLineCommandHandler
    : IRequestHandler<CreateWarehouseTransferLineCommand, BaseResponse<Guid>>
{
    private readonly IWarehouseTransferLineService _service;

    public CreateWarehouseTransferLineCommandHandler(IWarehouseTransferLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateWarehouseTransferLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
