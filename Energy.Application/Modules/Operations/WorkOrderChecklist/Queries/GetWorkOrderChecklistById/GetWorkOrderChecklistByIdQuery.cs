using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderChecklist.Queries.GetWorkOrderChecklistById;

/// <summary>Kimliğe göre WorkOrderChecklist detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWorkOrderChecklistByIdQuery(Guid Id)
    : IRequest<BaseResponse<WorkOrderChecklistDetailResponse>>;
