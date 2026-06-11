using FluentValidation;

namespace Energy.Application.Identity.Users.Commands.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Request.FirstName).NotEmpty();
        RuleFor(x => x.Request.LastName).NotEmpty();
        RuleFor(x => x.Request.UserName).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Request.Email).MaximumLength(256).When(x => !string.IsNullOrWhiteSpace(x.Request.Email));
        RuleFor(x => x.Request.Password).NotEmpty().MinimumLength(8);
    }
}
