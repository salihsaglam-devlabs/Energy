using Energy.Application.Modules.Inventory.StockDocumentLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Queries.GetStockDocumentLineList;

/// <summary>
/// <see cref="GetStockDocumentLineListQuery"/> handler'ı. <see cref="IStockDocumentLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockDocumentLineListQueryHandler
    : IRequestHandler<GetStockDocumentLineListQuery, BaseResponse<PaginatedResponse<StockDocumentLineListResponse>>>
{
    private readonly IStockDocumentLineService _service;

    public GetStockDocumentLineListQueryHandler(IStockDocumentLineService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<StockDocumentLineListResponse>>> Handle(
        GetStockDocumentLineListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
