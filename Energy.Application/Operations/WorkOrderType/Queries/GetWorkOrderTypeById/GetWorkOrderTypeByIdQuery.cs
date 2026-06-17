using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderType.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderType.Queries.GetWorkOrderTypeById;

/// <summary>Kimliğe göre WorkOrderType detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWorkOrderTypeByIdQuery(Guid Id)
    : IRequest<BaseResponse<WorkOrderTypeDetailResponse>>;
