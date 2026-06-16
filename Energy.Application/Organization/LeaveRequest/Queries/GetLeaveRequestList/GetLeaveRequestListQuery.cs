using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Requests;
using Energy.Shared.Models.V1.Organization.LeaveRequest.Responses;
using MediatR;

namespace Energy.Application.Organization.LeaveRequest.Queries.GetLeaveRequestList;

/// <summary>Sayfalanmış LeaveRequest listesini getiren sorgu.</summary>
/// <param name="Request">Sayfalama/filtre parametrelerini taşıyan istek modeli.</param>
public sealed record GetLeaveRequestListQuery(GetLeaveRequestListRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<LeaveRequestListResponse>>>;
