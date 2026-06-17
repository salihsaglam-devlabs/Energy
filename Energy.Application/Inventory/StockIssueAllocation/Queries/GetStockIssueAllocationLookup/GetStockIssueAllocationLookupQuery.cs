using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockIssueAllocation.Responses;
using MediatR;

namespace Energy.Application.Inventory.StockIssueAllocation.Queries.GetStockIssueAllocationLookup;

/// <summary>StockIssueAllocation lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetStockIssueAllocationLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<StockIssueAllocationLookupResponse>>>;
