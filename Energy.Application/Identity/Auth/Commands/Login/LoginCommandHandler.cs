using Energy.Application.Common.Exceptions;
using Energy.Application.Identity.Services;
using Energy.Localization;
using Energy.Shared.Models.V1.Common.Responses;
using Energy.Shared.Models.V1.Identity.Requests;
using Energy.Shared.Models.V1.Identity.Responses;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Energy.Application.Identity.Auth.Commands.Login;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, BaseResponse<AuthTokenResponse>>
{
    private readonly IUserService _userService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public LoginCommandHandler(
        IUserService userService,
        IJwtTokenService jwtTokenService,
        IStringLocalizer<SharedResource> localizer)
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
        _localizer = localizer;
    }

    public async Task<BaseResponse<AuthTokenResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var credentials = new ValidateCredentialsRequest
        {
            UserNameOrEmail = request.Request.UserNameOrEmail,
            Password = request.Request.Password
        };

        var validation = await _userService.ValidateCredentialsAsync(credentials, cancellationToken);

        if (!validation.IsAuthenticated || validation.UserId is null)
        {
            if (validation.IsLockedOut)
            {
                throw new ConflictException(_localizer.GetText(
                    LocalizationKeys.Auth.AccountLockedOut,
                    "The user account is locked out."));
            }

            return BaseResponse<AuthTokenResponse>.Failure(
                _localizer.GetText(LocalizationKeys.Auth.InvalidCredentials, "Invalid credentials."),
                [
                    _localizer.GetText(
                        LocalizationKeys.Auth.InvalidCredentialsDetail,
                        "User name/email or password is incorrect.")
                ]);
        }

        var permissions = await _userService.GetUserPermissionsAsync(validation.UserId.Value, cancellationToken);

        var token = _jwtTokenService.GenerateToken(
            validation.UserId.Value,
            validation.UserName,
            validation.Email,
            validation.Roles,
            validation.RoleKeys,
            permissions);

        return BaseResponse<AuthTokenResponse>.Success(
            token,
            _localizer.GetText(LocalizationKeys.Auth.LoginSuccessful, "Login successful."));
    }
}

