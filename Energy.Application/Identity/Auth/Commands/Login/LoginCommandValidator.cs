using FluentValidation;

namespace Energy.Application.Identity.Auth.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request.UserNameOrEmail).NotEmpty();
        RuleFor(x => x.Request.Password).NotEmpty();
    }
}

