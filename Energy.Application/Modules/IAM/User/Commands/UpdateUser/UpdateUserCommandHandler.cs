using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.Modules.IAM.User.Commands.UpdateUser;

/// <summary><see cref="UpdateUserCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class UpdateUserCommandHandler
    : IRequestHandler<UpdateUserCommand, BaseResponse<UserDetailResponse>>
{
    private readonly IUserService _users;

    public UpdateUserCommandHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<BaseResponse<UserDetailResponse>> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var result = await _users.UpdateAsync(request.Id, request.Request, ct);
        return BaseResponse<UserDetailResponse>.Success(result);
    }
}
