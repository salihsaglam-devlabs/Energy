using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Queries.GetUsers;

public sealed class GetUsersQuery : PaginatedRequest,
    IRequest<BaseResponse<PaginatedResponse<UserSummaryResponse>>>;
