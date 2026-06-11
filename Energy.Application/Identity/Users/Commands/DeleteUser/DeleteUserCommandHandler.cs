using Energy.Application.Identity.Services;
using Energy.Shared.Models.V1.Common.Responses;
using MediatR;

namespace Energy.Application.Identity.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler
    : IRequestHandler<DeleteUserCommand, BaseResponse<Guid>>
{
    private readonly IUserService _userService;

    public DeleteUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<BaseResponse<Guid>> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        await _userService.DeleteUserAsync(request.Id, cancellationToken);
        return BaseResponse<Guid>.Success(request.Id);
    }
}
