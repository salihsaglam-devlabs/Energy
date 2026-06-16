using FluentValidation;
using Energy.Shared.Models.V1.HR.TimesheetLine.Requests;

namespace Energy.Application.Modules.HR.TimesheetLine.Validators;

/// <summary>UpdateTimesheetLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateTimesheetLineRequestValidator : AbstractValidator<UpdateTimesheetLineRequest>
{
    public UpdateTimesheetLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TimesheetId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}
