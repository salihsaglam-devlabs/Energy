using FluentValidation;

namespace Energy.Application.Identity.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Request.Description).NotEmpty();
    }
}
