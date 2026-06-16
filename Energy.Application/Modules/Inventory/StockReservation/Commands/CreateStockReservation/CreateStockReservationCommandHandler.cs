using Energy.Application.Modules.Inventory.StockReservation.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockReservation.Commands.CreateStockReservation;

/// <summary>
/// <see cref="CreateStockReservationCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IStockReservationService"/>'i orkestre eder.
/// </summary>
public sealed class CreateStockReservationCommandHandler
    : IRequestHandler<CreateStockReservationCommand, BaseResponse<Guid>>
{
    private readonly IStockReservationService _service;

    public CreateStockReservationCommandHandler(IStockReservationService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateStockReservationCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
