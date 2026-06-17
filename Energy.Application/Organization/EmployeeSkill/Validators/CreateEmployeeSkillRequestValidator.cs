using FluentValidation;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Requests;

namespace Energy.Application.Organization.EmployeeSkill.Validators;

/// <summary>CreateEmployeeSkillRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateEmployeeSkillRequestValidator : AbstractValidator<CreateEmployeeSkillRequest>
{
    public CreateEmployeeSkillRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
