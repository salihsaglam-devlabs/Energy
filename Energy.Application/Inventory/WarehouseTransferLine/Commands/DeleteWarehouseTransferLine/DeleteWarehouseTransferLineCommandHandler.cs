using Energy.Application.Inventory.WarehouseTransferLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransferLine.Commands.DeleteWarehouseTransferLine;

/// <summary>
/// <see cref="DeleteWarehouseTransferLineCommand"/> handler'ı. <see cref="IWarehouseTransferLineService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteWarehouseTransferLineCommandHandler
    : IRequestHandler<DeleteWarehouseTransferLineCommand, BaseResponse<bool>>
{
    private readonly IWarehouseTransferLineService _service;

    public DeleteWarehouseTransferLineCommandHandler(IWarehouseTransferLineService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteWarehouseTransferLineCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
