using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Common.Requests;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using MediatR;

namespace Energy.Application.IAM.User.Commands.CreateUser;

/// <summary><see cref="CreateUserCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, BaseResponse<UserDetailResponse>>
{
    private readonly IUserService _users;

    public CreateUserCommandHandler(IUserService users)
    {
        _users = users;
    }

    public async Task<BaseResponse<UserDetailResponse>> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var result = await _users.CreateAsync(request.Request, ct);
        return BaseResponse<UserDetailResponse>.Success(result);
    }
}
