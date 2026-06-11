using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, BaseResponse<UserDetailResponse>>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<UserDetailResponse>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.UpdateUserAsync(request.Id, request.Request, cancellationToken);
        return BaseResponse<UserDetailResponse>.Success(result);
    }
}
