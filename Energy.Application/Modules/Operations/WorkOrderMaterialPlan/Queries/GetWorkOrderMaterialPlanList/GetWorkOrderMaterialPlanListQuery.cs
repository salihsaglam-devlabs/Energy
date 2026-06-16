using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Queries.GetWorkOrderMaterialPlanList;

/// <summary>Sayfalanmış WorkOrderMaterialPlan listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWorkOrderMaterialPlanListQuery(GetWorkOrderMaterialPlanListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WorkOrderMaterialPlanListResponse>>>;
