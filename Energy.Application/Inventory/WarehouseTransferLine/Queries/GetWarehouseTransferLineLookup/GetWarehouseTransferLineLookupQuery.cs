using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransferLine.Queries.GetWarehouseTransferLineLookup;

/// <summary>WarehouseTransferLine lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetWarehouseTransferLineLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<WarehouseTransferLineLookupResponse>>>;
