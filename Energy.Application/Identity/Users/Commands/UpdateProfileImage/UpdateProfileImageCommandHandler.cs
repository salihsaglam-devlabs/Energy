using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.UpdateProfileImage;

public sealed class UpdateProfileImageCommandHandler
    : IRequestHandler<UpdateProfileImageCommand, BaseResponse<UserDetailResponse>>
{
    private readonly IUserService _userService;

    public UpdateProfileImageCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<UserDetailResponse>> Handle(
        UpdateProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.SetProfileImageAsync(
            request.UserId,
            request.Content,
            request.ContentType,
            cancellationToken);
        return BaseResponse<UserDetailResponse>.Success(result);
    }
}

