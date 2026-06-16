using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Warehouse.Requests;
using Energy.Shared.Models.V1.Inventory.Warehouse.Responses;
using MediatR;

namespace Energy.Application.Inventory.Warehouse.Queries.GetWarehouseList;

/// <summary>Sayfalanmış Warehouse listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWarehouseListQuery(GetWarehouseListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WarehouseListResponse>>>;
