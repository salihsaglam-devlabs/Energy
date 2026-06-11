using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, BaseResponse<UserDetailResponse>>
{
    private readonly IUserService _userService;

    public GetUserByIdQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<UserDetailResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.GetUserByIdAsync(request.Id, cancellationToken);
        return BaseResponse<UserDetailResponse>.Success(result);
    }
}
