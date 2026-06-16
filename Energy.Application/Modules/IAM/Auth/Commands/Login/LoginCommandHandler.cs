using Energy.Application.Common.Exceptions;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using Energy.Application.Identity.Services;
using Microsoft.Extensions.Localization;
using MediatR;

namespace Energy.Application.Modules.IAM.Auth.Commands.Login;

/// <summary><see cref="LoginCommand"/> handler'ı (orkestrasyon).</summary>
public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, BaseResponse<AuthTokenResponse>>
{
    private readonly IUserService _users;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public LoginCommandHandler(IUserService users, IStringLocalizer<SharedResource> localizer)
    {
        _users = users;
        _localizer = localizer;
    }

    public async Task<BaseResponse<AuthTokenResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        var token = await _users.LoginAsync(request.Request, ct);
        return token is null
            ? BaseResponse<AuthTokenResponse>.Failure(_localizer[LocalizationKeys.Messages.InvalidCredentials].Value)
            : BaseResponse<AuthTokenResponse>.Success(token);
    }
}
