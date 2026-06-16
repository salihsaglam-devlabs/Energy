using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockDocumentType.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockDocumentType.Queries.GetStockDocumentTypeLookup;

/// <summary>StockDocumentType lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetStockDocumentTypeLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<StockDocumentTypeLookupResponse>>>;
