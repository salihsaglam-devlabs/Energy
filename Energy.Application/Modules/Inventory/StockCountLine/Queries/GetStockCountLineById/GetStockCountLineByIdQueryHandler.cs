using Energy.Application.Modules.Inventory.StockCountLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCountLine.Queries.GetStockCountLineById;

/// <summary>
/// <see cref="GetStockCountLineByIdQuery"/> handler'ı. <see cref="IStockCountLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockCountLineByIdQueryHandler
    : IRequestHandler<GetStockCountLineByIdQuery, BaseResponse<StockCountLineDetailResponse>>
{
    private readonly IStockCountLineService _service;

    public GetStockCountLineByIdQueryHandler(IStockCountLineService service)
        => _service = service;

    public Task<BaseResponse<StockCountLineDetailResponse>> Handle(
        GetStockCountLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
