using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockIssueAllocation.Queries.GetStockIssueAllocationById;

/// <summary>Kimliğe göre StockIssueAllocation detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetStockIssueAllocationByIdQuery(Guid Id)
    : IRequest<BaseResponse<StockIssueAllocationDetailResponse>>;
