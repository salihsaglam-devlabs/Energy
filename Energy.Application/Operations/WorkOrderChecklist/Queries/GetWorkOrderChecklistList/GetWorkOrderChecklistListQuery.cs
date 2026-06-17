using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Responses;
using MediatR;

namespace Energy.Application.Operations.WorkOrderChecklist.Queries.GetWorkOrderChecklistList;

/// <summary>Sayfalanmış WorkOrderChecklist listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWorkOrderChecklistListQuery(GetWorkOrderChecklistListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WorkOrderChecklistListResponse>>>;
