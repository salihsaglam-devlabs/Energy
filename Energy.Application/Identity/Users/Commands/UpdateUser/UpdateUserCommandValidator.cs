using FluentValidation;

namespace Energy.Application.Identity.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Request.FirstName).NotEmpty();
        RuleFor(x => x.Request.LastName).NotEmpty();
        RuleFor(x => x.Request.UserName).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Request.Email).MaximumLength(256).When(x => !string.IsNullOrWhiteSpace(x.Request.Email));
    }
}
