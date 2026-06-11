using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Queries.GetProfileImage;

public sealed class GetProfileImageQueryHandler
    : IRequestHandler<GetProfileImageQuery, BaseResponse<ProfileImageResponse?>>
{
    private readonly IUserService _userService;

    public GetProfileImageQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<ProfileImageResponse?>> Handle(
        GetProfileImageQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.GetProfileImageAsync(request.UserId, cancellationToken);
        return BaseResponse<ProfileImageResponse?>.Success(result);
    }
}

