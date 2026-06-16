using FluentValidation;
using Energy.Shared.Models.V1.HR.Timesheet.Requests;

namespace Energy.Application.Modules.HR.Timesheet.Validators;

/// <summary>UpdateTimesheetRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateTimesheetRequestValidator : AbstractValidator<UpdateTimesheetRequest>
{
    public UpdateTimesheetRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TimesheetNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
