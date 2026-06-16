using FluentValidation;
using Energy.Shared.Models.V1.IAM.Role.Requests;

namespace Energy.Application.Modules.IAM.Role.Validators;

/// <summary>CreateRoleRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}
