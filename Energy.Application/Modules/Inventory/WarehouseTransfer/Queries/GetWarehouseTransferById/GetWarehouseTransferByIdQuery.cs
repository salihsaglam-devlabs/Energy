using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Responses;
using MediatR;

namespace Energy.Application.Modules.Inventory.WarehouseTransfer.Queries.GetWarehouseTransferById;

/// <summary>Kimliğe göre WarehouseTransfer detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWarehouseTransferByIdQuery(Guid Id)
    : IRequest<BaseResponse<WarehouseTransferDetailResponse>>;
