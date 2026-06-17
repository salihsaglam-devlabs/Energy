using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderMaterialPlan.Queries.GetWorkOrderMaterialPlanById;

/// <summary>Kimliğe göre WorkOrderMaterialPlan detayını getiren sorgu.</summary>
/// <param name="Id">İstenen kaydın kimliği.</param>
public sealed record GetWorkOrderMaterialPlanByIdQuery(Guid Id)
    : IRequest<BaseResponse<WorkOrderMaterialPlanDetailResponse>>;
