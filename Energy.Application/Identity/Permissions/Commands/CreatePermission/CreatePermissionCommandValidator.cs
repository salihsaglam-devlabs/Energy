using FluentValidation;

namespace Energy.Application.Identity.Permissions.Commands.CreatePermission;

public sealed class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(x => x.Request.Code).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Request.Name).NotEmpty().MaximumLength(200);
    }
}

