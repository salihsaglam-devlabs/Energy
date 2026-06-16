using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocument.Queries.GetStockDocumentById;

/// <summary>Kimliğe göre StockDocument detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetStockDocumentByIdQuery(Guid Id)
    : IRequest<BaseResponse<StockDocumentDetailResponse>>;
