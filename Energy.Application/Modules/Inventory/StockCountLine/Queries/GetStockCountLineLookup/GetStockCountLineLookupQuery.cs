using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockCountLine.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockCountLine.Queries.GetStockCountLineLookup;

/// <summary>StockCountLine lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetStockCountLineLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<StockCountLineLookupResponse>>>;
