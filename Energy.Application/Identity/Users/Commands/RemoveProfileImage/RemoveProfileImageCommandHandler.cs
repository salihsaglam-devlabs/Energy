using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.RemoveProfileImage;

public sealed class RemoveProfileImageCommandHandler
    : IRequestHandler<RemoveProfileImageCommand, BaseResponse<UserDetailResponse>>
{
    private readonly IUserService _userService;

    public RemoveProfileImageCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<UserDetailResponse>> Handle(
        RemoveProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.RemoveProfileImageAsync(request.UserId, cancellationToken);
        return BaseResponse<UserDetailResponse>.Success(result);
    }
}

