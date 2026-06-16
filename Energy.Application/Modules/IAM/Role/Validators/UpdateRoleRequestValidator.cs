using FluentValidation;
using Energy.Shared.Models.V1.IAM.Role.Requests;

namespace Energy.Application.Modules.IAM.Role.Validators;

/// <summary>UpdateRoleRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
