using Energy.Application.Modules.Inventory.StockDocumentType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentType.Queries.GetStockDocumentTypeList;

/// <summary>
/// <see cref="GetStockDocumentTypeListQuery"/> handler'ı. <see cref="IStockDocumentTypeService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockDocumentTypeListQueryHandler
    : IRequestHandler<GetStockDocumentTypeListQuery, BaseResponse<PaginatedResponse<StockDocumentTypeListResponse>>>
{
    private readonly IStockDocumentTypeService _service;

    public GetStockDocumentTypeListQueryHandler(IStockDocumentTypeService service)
        => _service = service;

    public Task<BaseResponse<PaginatedResponse<StockDocumentTypeListResponse>>> Handle(
        GetStockDocumentTypeListQuery request,
        CancellationToken cancellationToken)
        => _service.GetListAsync(request.Request, cancellationToken);
}
