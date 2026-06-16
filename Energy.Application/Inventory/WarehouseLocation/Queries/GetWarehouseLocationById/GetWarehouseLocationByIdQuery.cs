using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseLocation.Queries.GetWarehouseLocationById;

/// <summary>Kimliğe göre WarehouseLocation detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWarehouseLocationByIdQuery(Guid Id)
    : IRequest<BaseResponse<WarehouseLocationDetailResponse>>;
