using FluentValidation;
using Energy.Shared.Models.V1.IAM.UserPermission.Requests;

namespace Energy.Application.IAM.UserPermission.Validators;

/// <summary>UpdateUserPermissionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateUserPermissionRequestValidator : AbstractValidator<UpdateUserPermissionRequest>
{
    public UpdateUserPermissionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.PermissionCode).NotEmpty();
    }
}
