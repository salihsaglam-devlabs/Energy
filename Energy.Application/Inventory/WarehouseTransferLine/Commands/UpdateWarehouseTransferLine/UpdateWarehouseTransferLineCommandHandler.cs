using Energy.Application.Inventory.WarehouseTransferLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransferLine.Commands.UpdateWarehouseTransferLine;

/// <summary>
/// <see cref="UpdateWarehouseTransferLineCommand"/> handler'ı. <see cref="IWarehouseTransferLineService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateWarehouseTransferLineCommandHandler
    : IRequestHandler<UpdateWarehouseTransferLineCommand, BaseResponse<bool>>
{
    private readonly IWarehouseTransferLineService _service;

    public UpdateWarehouseTransferLineCommandHandler(IWarehouseTransferLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateWarehouseTransferLineCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
