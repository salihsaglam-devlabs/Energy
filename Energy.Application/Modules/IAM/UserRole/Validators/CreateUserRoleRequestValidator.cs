using FluentValidation;
using Energy.Shared.Models.V1.IAM.UserRole.Requests;

namespace Energy.Application.Modules.IAM.UserRole.Validators;

/// <summary>CreateUserRoleRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateUserRoleRequestValidator : AbstractValidator<CreateUserRoleRequest>
{
    public CreateUserRoleRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.RoleId).NotEmpty();
    }
}
