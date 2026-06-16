using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Requests;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseLocation.Queries.GetWarehouseLocationList;

/// <summary>Sayfalanmış WarehouseLocation listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWarehouseLocationListQuery(GetWarehouseLocationListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WarehouseLocationListResponse>>>;
