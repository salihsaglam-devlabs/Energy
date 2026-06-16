using Energy.Application.Inventory.StockDocument.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocument.Queries.GetStockDocumentList;

/// <summary>
/// <see cref="GetStockDocumentListQuery"/> handler'ı. <see cref="IStockDocumentService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockDocumentListQueryHandler
    : IRequestHandler<GetStockDocumentListQuery, BaseResponse<PaginatedResponse<StockDocumentListResponse>>>
{
    private readonly IStockDocumentService _service;

    public GetStockDocumentListQueryHandler(IStockDocumentService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<StockDocumentListResponse>>> Handle(
        GetStockDocumentListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
