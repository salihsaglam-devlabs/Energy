using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Requests;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransferLine.Queries.GetWarehouseTransferLineList;

/// <summary>Sayfalanmış WarehouseTransferLine listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWarehouseTransferLineListQuery(GetWarehouseTransferLineListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WarehouseTransferLineListResponse>>>;
