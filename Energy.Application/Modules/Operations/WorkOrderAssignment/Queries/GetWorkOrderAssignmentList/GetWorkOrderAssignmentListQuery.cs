using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Requests;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Responses;
using MediatR;

namespace Energy.Application.Modules.Operations.WorkOrderAssignment.Queries.GetWorkOrderAssignmentList;

/// <summary>Sayfalanmış WorkOrderAssignment listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetWorkOrderAssignmentListQuery(GetWorkOrderAssignmentListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<WorkOrderAssignmentListResponse>>>;
