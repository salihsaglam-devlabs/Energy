using FluentValidation;

namespace Energy.Application.Identity.Users.Commands.SetUserRoles;

public sealed class SetUserRolesCommandValidator : AbstractValidator<SetUserRolesCommand>
{
    public SetUserRolesCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleForEach(x => x.RoleIds).NotEmpty();
    }
}
