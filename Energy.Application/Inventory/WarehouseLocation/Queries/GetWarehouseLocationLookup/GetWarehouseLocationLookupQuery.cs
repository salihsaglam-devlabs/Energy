using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseLocation.Queries.GetWarehouseLocationLookup;

/// <summary>WarehouseLocation lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetWarehouseLocationLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<WarehouseLocationLookupResponse>>>;
