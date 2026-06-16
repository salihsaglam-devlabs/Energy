using Energy.Application.Modules.Inventory.StockCount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCount.Commands.CreateStockCount;

/// <summary>
/// <see cref="CreateStockCountCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IStockCountService"/>'i orkestre eder.
/// </summary>
public sealed class CreateStockCountCommandHandler
    : IRequestHandler<CreateStockCountCommand, BaseResponse<Guid>>
{
    private readonly IStockCountService _service;

    public CreateStockCountCommandHandler(IStockCountService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateStockCountCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
