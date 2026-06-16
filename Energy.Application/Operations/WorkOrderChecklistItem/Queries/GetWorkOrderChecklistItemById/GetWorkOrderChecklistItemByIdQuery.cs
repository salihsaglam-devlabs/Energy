using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklistItem.Queries.GetWorkOrderChecklistItemById;

/// <summary>Kimliğe göre WorkOrderChecklistItem detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWorkOrderChecklistItemByIdQuery(Guid Id)
    : IRequest<BaseResponse<WorkOrderChecklistItemDetailResponse>>;
