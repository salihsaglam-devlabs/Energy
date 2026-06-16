using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Responses;
using MediatR;

namespace Energy.Application.Inventory.WarehouseTransferLine.Queries.GetWarehouseTransferLineById;

/// <summary>Kimliğe göre WarehouseTransferLine detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWarehouseTransferLineByIdQuery(Guid Id)
    : IRequest<BaseResponse<WarehouseTransferLineDetailResponse>>;
