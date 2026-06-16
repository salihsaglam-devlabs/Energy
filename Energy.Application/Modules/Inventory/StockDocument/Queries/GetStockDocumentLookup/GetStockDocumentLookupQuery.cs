using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocument.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocument.Queries.GetStockDocumentLookup;

/// <summary>StockDocument lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetStockDocumentLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<StockDocumentLookupResponse>>>;
