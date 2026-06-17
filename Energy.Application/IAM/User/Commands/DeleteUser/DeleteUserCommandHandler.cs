using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.IAM.User.Commands.DeleteUser;

/// <summary><see cref="DeleteUserCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class DeleteUserCommandHandler
    : IRequestHandler<DeleteUserCommand, BaseResponse<bool>>
{
    private readonly IUserService _users;

    public DeleteUserCommandHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        var result = await _users.DeleteAsync(request.Id, ct);
        return BaseResponse<bool>.Success(result);
    }
}
