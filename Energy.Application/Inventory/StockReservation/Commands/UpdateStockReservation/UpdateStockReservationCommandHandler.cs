using Energy.Application.Inventory.StockReservation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockReservation.Commands.UpdateStockReservation;

/// <summary>
/// <see cref="UpdateStockReservationCommand"/> handler'ı. <see cref="IStockReservationService"/>'i orkestre eder.
/// </summary>
public sealed class UpdateStockReservationCommandHandler
    : IRequestHandler<UpdateStockReservationCommand, BaseResponse<bool>>
{
    private readonly IStockReservationService _service;

    public UpdateStockReservationCommandHandler(IStockReservationService service)
        => _service = service;

    public Task<BaseResponse<bool>> Handle(
        UpdateStockReservationCommand request,
        CancellationToken cancellationToken)
        => _service.UpdateAsync(request.Id, request.Request, cancellationToken);
}
