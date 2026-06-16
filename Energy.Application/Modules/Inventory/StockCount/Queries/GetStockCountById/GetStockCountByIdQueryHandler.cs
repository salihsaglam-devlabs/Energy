using Energy.Application.Modules.Inventory.StockCount.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCount.Queries.GetStockCountById;

/// <summary>
/// <see cref="GetStockCountByIdQuery"/> handler'ı. <see cref="IStockCountService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockCountByIdQueryHandler
    : IRequestHandler<GetStockCountByIdQuery, BaseResponse<StockCountDetailResponse>>
{
    private readonly IStockCountService _service;

    public GetStockCountByIdQueryHandler(IStockCountService service)
        => _service = service;

    public Task<BaseResponse<StockCountDetailResponse>> Handle(
        GetStockCountByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
