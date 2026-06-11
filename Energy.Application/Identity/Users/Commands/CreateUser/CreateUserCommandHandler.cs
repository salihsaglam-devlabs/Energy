using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, BaseResponse<UserDetailResponse>>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<UserDetailResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _userService.CreateUserAsync(request.Request, cancellationToken);
        return BaseResponse<UserDetailResponse>.Success(result);
    }
}
