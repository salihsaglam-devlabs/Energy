using FluentValidation;

namespace Energy.Application.Identity.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Request.Description).NotEmpty();
    }
}
