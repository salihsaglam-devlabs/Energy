using Energy.Application.Inventory.StockDocumentLine.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocumentLine.Queries.GetStockDocumentLineById;

/// <summary>
/// <see cref="GetStockDocumentLineByIdQuery"/> handler'ı. <see cref="IStockDocumentLineService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockDocumentLineByIdQueryHandler
    : IRequestHandler<GetStockDocumentLineByIdQuery, BaseResponse<StockDocumentLineDetailResponse>>
{
    private readonly IStockDocumentLineService _service;

    public GetStockDocumentLineByIdQueryHandler(IStockDocumentLineService service)
        => _service = service;

    public Task<BaseResponse<StockDocumentLineDetailResponse>> Handle(
        GetStockDocumentLineByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
