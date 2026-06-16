using Energy.Application.Modules.Inventory.StockCountLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCountLine.Commands.CreateStockCountLine;

/// <summary>
/// <see cref="CreateStockCountLineCommand"/> handler'ı. İş mantığı içermez; yalnızca
/// <see cref="IStockCountLineService"/>'i orkestre eder.
/// </summary>
public sealed class CreateStockCountLineCommandHandler
    : IRequestHandler<CreateStockCountLineCommand, BaseResponse<Guid>>
{
    private readonly IStockCountLineService _service;

    public CreateStockCountLineCommandHandler(IStockCountLineService service)
        => _service = service;

    public Task<BaseResponse<Guid>> Handle(
        CreateStockCountLineCommand request,
        CancellationToken cancellationToken)
        => _service.CreateAsync(request.Request, cancellationToken);
}
