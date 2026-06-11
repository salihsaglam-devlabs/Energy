using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.UpdateUserPassword;

public sealed class UpdateUserPasswordCommandHandler
    : IRequestHandler<UpdateUserPasswordCommand, BaseResponse<Guid>>
{
    private readonly IUserService _userService;

    public UpdateUserPasswordCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<Guid>> Handle(
        UpdateUserPasswordCommand request,
        CancellationToken cancellationToken)
    {
        await _userService.UpdatePasswordAsync(request.Id, request.NewPassword, cancellationToken);
        return BaseResponse<Guid>.Success(request.Id);
    }
}
