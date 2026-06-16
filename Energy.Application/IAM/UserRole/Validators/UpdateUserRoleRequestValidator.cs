using FluentValidation;
using Energy.Shared.Models.V1.IAM.UserRole.Requests;

namespace Energy.Application.IAM.UserRole.Validators;

/// <summary>UpdateUserRoleRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateUserRoleRequestValidator : AbstractValidator<UpdateUserRoleRequest>
{
    public UpdateUserRoleRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}
