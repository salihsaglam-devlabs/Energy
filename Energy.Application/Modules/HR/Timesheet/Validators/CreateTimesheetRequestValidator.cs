using FluentValidation;
using Energy.Shared.Models.V1.HR.Timesheet.Requests;

namespace Energy.Application.Modules.HR.Timesheet.Validators;

/// <summary>CreateTimesheetRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateTimesheetRequestValidator : AbstractValidator<CreateTimesheetRequest>
{
    public CreateTimesheetRequestValidator()
    {
        RuleFor(x => x.TimesheetNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
