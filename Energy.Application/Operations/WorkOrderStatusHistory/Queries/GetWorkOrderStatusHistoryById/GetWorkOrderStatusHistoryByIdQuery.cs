using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderStatusHistory.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderStatusHistory.Queries.GetWorkOrderStatusHistoryById;

/// <summary>Kimliğe göre WorkOrderStatusHistory detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWorkOrderStatusHistoryByIdQuery(Guid Id)
    : IRequest<BaseResponse<WorkOrderStatusHistoryDetailResponse>>;
