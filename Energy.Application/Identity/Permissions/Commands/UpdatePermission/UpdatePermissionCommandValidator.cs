using FluentValidation;

namespace Energy.Application.Identity.Permissions.Commands.UpdatePermission;

public sealed class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator()
    {
        RuleFor(x => x.Request.Code).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(200);
    }
}

