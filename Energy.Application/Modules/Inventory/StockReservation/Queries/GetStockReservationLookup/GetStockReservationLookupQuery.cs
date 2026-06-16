using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.StockReservation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.StockReservation.Queries.GetStockReservationLookup;

/// <summary>StockReservation lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetStockReservationLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<StockReservationLookupResponse>>>;
