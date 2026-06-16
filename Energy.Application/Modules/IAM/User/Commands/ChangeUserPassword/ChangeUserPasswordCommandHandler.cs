using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Commands.ChangeUserPassword;

/// <summary><see cref="ChangeUserPasswordCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class ChangeUserPasswordCommandHandler
    : IRequestHandler<ChangeUserPasswordCommand, BaseResponse<bool>>
{
    private readonly IUserService _users;

    public ChangeUserPasswordCommandHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<BaseResponse<bool>> Handle(ChangeUserPasswordCommand request, CancellationToken ct)
    {
        var result = await _users.ChangePasswordAsync(request.Id, request.Request, ct);
        return BaseResponse<bool>.Success(result);
    }
}
