using Energy.Application.Inventory.StockDocumentType.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockDocumentType.Queries.GetStockDocumentTypeById;

/// <summary>
/// <see cref="GetStockDocumentTypeByIdQuery"/> handler'ı. <see cref="IStockDocumentTypeService"/>'i orkestre eder.
/// </summary>
public sealed class GetStockDocumentTypeByIdQueryHandler
    : IRequestHandler<GetStockDocumentTypeByIdQuery, BaseResponse<StockDocumentTypeDetailResponse>>
{
    private readonly IStockDocumentTypeService _service;

    public GetStockDocumentTypeByIdQueryHandler(IStockDocumentTypeService service)
        => _service = service;

    public Task<BaseResponse<StockDocumentTypeDetailResponse>> Handle(
        GetStockDocumentTypeByIdQuery request,
        CancellationToken cancellationToken)
        => _service.GetByIdAsync(request.Id, cancellationToken);
}
