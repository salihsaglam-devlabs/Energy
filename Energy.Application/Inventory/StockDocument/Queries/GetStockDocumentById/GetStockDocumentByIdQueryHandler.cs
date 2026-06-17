using Energy.Application.Inventory.StockDocument.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocument.Queries.GetStockDocumentById;

/// <summary>
/// <see cref="GetStockDocumentByIdQuery"/> handler'ı. <see cref="IStockDocumentService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockDocumentByIdQueryHandler
    : IRequestHandler<GetStockDocumentByIdQuery, BaseResponse<StockDocumentDetailResponse>>
{
    private readonly IStockDocumentService _service;

    public GetStockDocumentByIdQueryHandler(IStockDocumentService service)
        => _service = service;

    public Task<BaseResponse<StockDocumentDetailResponse>> Handle(
        GetStockDocumentByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
