using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockCountLine.Queries.GetStockCountLineById;

/// <summary>Kimliğe göre StockCountLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetStockCountLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<StockCountLineDetailResponse>>;
