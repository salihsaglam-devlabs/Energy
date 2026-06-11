using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Queries.GetAdminPermissionHealth;

public sealed class GetAdminPermissionHealthQueryHandler
    : IRequestHandler<GetAdminPermissionHealthQuery, BaseResponse<AdminPermissionHealthResponse>>
{
    private readonly IUserService _userService;

    public GetAdminPermissionHealthQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<AdminPermissionHealthResponse>> Handle(
        GetAdminPermissionHealthQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.GetAdminPermissionHealthAsync(cancellationToken);
        return BaseResponse<AdminPermissionHealthResponse>.Success(result);
    }
}

