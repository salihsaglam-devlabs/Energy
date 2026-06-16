using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.IAM.User.Queries.GetUserList;

/// <summary><see cref="GetUserListQuery"/> handler'ı (orkestrasyon).</summary>
public sealed class GetUserListQueryHandler
    : IRequestHandler<GetUserListQuery, BaseResponse<PaginatedResponse<UserSummaryResponse>>>
{
    private readonly IUserService _users;

    public GetUserListQueryHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<BaseResponse<PaginatedResponse<UserSummaryResponse>>> Handle(GetUserListQuery request, CancellationToken ct)
    {
        var result = await _users.GetAllAsync(request.Request, ct);
        return BaseResponse<PaginatedResponse<UserSummaryResponse>>.Success(result);
    }
}
