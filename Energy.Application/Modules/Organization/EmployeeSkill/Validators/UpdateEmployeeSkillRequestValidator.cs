using FluentValidation;
using Energy.Shared.Models.V1.Organization.EmployeeSkill.Requests;

namespace Energy.Application.Modules.Organization.EmployeeSkill.Validators;

/// <summary>UpdateEmployeeSkillRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateEmployeeSkillRequestValidator : AbstractValidator<UpdateEmployeeSkillRequest>
{
    public UpdateEmployeeSkillRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
