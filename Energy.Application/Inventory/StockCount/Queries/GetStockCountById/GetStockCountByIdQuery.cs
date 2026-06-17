using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCount.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockCount.Queries.GetStockCountById;

/// <summary>Kimliğe göre StockCount detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetStockCountByIdQuery(Guid Id)
    : IRequest<BaseResponse<StockCountDetailResponse>>;
