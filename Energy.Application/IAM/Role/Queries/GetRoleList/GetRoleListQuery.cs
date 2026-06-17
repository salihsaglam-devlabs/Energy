using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.IAM.Role.Queries.GetRoleList;

/// <summary>GetRoleList</summary>
public sealed record GetRoleListQuery(PaginatedRequest Request)
    : IRequest<BaseResponse<PaginatedResponse<RoleSummaryResponse>>>;
