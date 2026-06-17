using Energy.Application.Inventory.StockReservation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockReservation.Commands.DeleteStockReservation;

/// <summary>
/// <see cref="DeleteStockReservationCommand"/> handler'ı. <see cref="IStockReservationService"/>'i orkestre eder.
/// </summary>
public sealed class DeleteStockReservationCommandHandler
    : IRequestHandler<DeleteStockReservationCommand, BaseResponse<bool>>
{
    private readonly IStockReservationService _service;

    public DeleteStockReservationCommandHandler(IStockReservationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        DeleteStockReservationCommand request,
        CancellationToken cancellationToken)
        => _service.DeleteAsync(request.Id, cancellationToken);
}
