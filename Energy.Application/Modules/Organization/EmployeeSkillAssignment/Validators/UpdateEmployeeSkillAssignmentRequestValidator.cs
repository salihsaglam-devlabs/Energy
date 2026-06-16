using FluentValidation;
using Energy.Shared.Models.V1.Organization.EmployeeSkillAssignment.Requests;

namespace Energy.Application.Modules.Organization.EmployeeSkillAssignment.Validators;

/// <summary>UpdateEmployeeSkillAssignmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateEmployeeSkillAssignmentRequestValidator : AbstractValidator<UpdateEmployeeSkillAssignmentRequest>
{
    public UpdateEmployeeSkillAssignmentRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.EmployeeSkillId).NotEmpty();
    }
}
