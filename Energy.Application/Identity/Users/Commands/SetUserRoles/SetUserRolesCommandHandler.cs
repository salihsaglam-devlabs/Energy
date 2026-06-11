using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.SetUserRoles;

public sealed class SetUserRolesCommandHandler
    : IRequestHandler<SetUserRolesCommand, BaseResponse<UserDetailResponse>>
{
    private readonly IUserService _userService;

    public SetUserRolesCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<UserDetailResponse>> Handle(
        SetUserRolesCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.SetUserRolesAsync(request.Id, request.RoleIds, cancellationToken);
        return BaseResponse<UserDetailResponse>.Success(result);
    }
}
