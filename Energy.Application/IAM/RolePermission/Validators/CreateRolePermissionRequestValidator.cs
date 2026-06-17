using FluentValidation;
using Energy.Shared.Models.V1.IAM.RolePermission.Requests;

namespace Energy.Application.IAM.RolePermission.Validators;

/// <summary>CreateRolePermissionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateRolePermissionRequestValidator : AbstractValidator<CreateRolePermissionRequest>
{
    public CreateRolePermissionRequestValidator()
    {
        RuleFor(x => x.RoleId).NotEmpty();
        RuleFor(x => x.PermissionCode).NotEmpty();
    }
}
