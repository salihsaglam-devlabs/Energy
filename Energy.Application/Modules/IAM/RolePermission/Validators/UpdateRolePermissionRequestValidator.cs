using FluentValidation;
using Energy.Shared.Models.V1.IAM.RolePermission.Requests;

namespace Energy.Application.Modules.IAM.RolePermission.Validators;

/// <summary>UpdateRolePermissionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateRolePermissionRequestValidator : AbstractValidator<UpdateRolePermissionRequest>
{
    public UpdateRolePermissionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
        RuleFor(x => x.PermissionCode).NotEmpty();
    }
}
