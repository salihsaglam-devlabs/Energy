using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Roles.Queries.GetRoles;

public sealed class GetRolesQuery : PaginatedRequest,
    IRequest<BaseResponse<PaginatedResponse<RoleSummaryResponse>>>;
