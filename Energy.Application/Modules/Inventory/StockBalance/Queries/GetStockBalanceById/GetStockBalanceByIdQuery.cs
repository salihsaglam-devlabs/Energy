using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockBalance.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockBalance.Queries.GetStockBalanceById;

/// <summary>Kimliğe göre StockBalance detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetStockBalanceByIdQuery(Guid Id)
    : IRequest<BaseResponse<StockBalanceDetailResponse>>;
