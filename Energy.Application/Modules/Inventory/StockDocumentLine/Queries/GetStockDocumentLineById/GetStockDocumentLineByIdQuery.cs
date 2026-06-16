using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentLine.Queries.GetStockDocumentLineById;

/// <summary>Kimliğe göre StockDocumentLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetStockDocumentLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<StockDocumentLineDetailResponse>>;
