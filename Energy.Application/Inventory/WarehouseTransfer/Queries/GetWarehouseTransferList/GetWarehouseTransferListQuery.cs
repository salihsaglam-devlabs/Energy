using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Requests;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransfer.Queries.GetWarehouseTransferList;

/// <summary>Sayfalanmış WarehouseTransfer listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWarehouseTransferListQuery(GetWarehouseTransferListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WarehouseTransferListResponse>>>;
