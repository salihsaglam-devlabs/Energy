using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.Warehouse.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.Warehouse.Queries.GetWarehouseById;

/// <summary>Kimliğe göre Warehouse detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWarehouseByIdQuery(Guid Id)
    : IRequest<BaseResponse<WarehouseDetailResponse>>;
