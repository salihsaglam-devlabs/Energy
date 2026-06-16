using Energy.Application.Modules.Inventory.StockBalance.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockBalance.Queries.GetStockBalanceById;

/// <summary>
/// <see cref="GetStockBalanceByIdQuery"/> handler'ı. <see cref="IStockBalanceService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockBalanceByIdQueryHandler
    : IRequestHandler<GetStockBalanceByIdQuery, BaseResponse<StockBalanceDetailResponse>>
{
    private readonly IStockBalanceService _service;

    public GetStockBalanceByIdQueryHandler(IStockBalanceService service)
        => _service = service;

    public Task<BaseResponse<StockBalanceDetailResponse>> Handle(
        GetStockBalanceByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
