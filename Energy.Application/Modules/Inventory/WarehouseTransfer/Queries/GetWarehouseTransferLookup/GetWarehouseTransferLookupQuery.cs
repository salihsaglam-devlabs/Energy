using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseTransfer.Queries.GetWarehouseTransferLookup;

/// <summary>WarehouseTransfer lookup listesini (arama + aktiflik filtreli) getiren sorgu.</summary>
/// <param name="Search">Opsiyonel arama metni.</param>
/// <param name="ActiveOnly">Yalnızca aktif kayıtlar getirilsin mi.</param>
public sealed record GetWarehouseTransferLookupQuery(string? Search, bool ActiveOnly)
    : IRequest<BaseResponse<IReadOnlyList<WarehouseTransferLookupResponse>>>;
