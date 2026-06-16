using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockLot.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockLot.Queries.GetStockLotById;

/// <summary>Kimliğe göre StockLot detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetStockLotByIdQuery(Guid Id)
    : IRequest<BaseResponse<StockLotDetailResponse>>;
