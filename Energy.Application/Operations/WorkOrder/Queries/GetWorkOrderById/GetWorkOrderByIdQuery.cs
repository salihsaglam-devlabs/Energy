using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrder.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrder.Queries.GetWorkOrderById;

/// <summary>Kimliğe göre WorkOrder detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWorkOrderByIdQuery(Guid Id)
    : IRequest<BaseResponse<WorkOrderDetailResponse>>;
