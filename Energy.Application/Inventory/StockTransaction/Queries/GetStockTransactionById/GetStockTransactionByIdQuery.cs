using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockTransaction.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockTransaction.Queries.GetStockTransactionById;

/// <summary>Kimliğe göre StockTransaction detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetStockTransactionByIdQuery(Guid Id)
    : IRequest<BaseResponse<StockTransactionDetailResponse>>;
