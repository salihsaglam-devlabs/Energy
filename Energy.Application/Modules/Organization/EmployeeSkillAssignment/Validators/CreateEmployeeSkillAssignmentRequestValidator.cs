using FluentValidation;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Requests;

namespace Energy.Application.Modules.Organization.EmployeeSkillAssignment.Validators;

/// <summary>CreateEmployeeSkillAssignmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateEmployeeSkillAssignmentRequestValidator : AbstractValidator<CreateEmployeeSkillAssignmentRequest>
{
    public CreateEmployeeSkillAssignmentRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.EmployeeSkillId).NotEmpty();
    }
}
