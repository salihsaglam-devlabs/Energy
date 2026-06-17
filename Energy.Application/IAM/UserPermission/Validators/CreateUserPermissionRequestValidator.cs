using FluentValidation;
using Energy.Shared.Models.V1.IAM.UserPermission.Requests;

namespace Energy.Application.IAM.UserPermission.Validators;

/// <summary>CreateUserPermissionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateUserPermissionRequestValidator : AbstractValidator<CreateUserPermissionRequest>
{
    public CreateUserPermissionRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PermissionCode).NotEmpty();
    }
}
