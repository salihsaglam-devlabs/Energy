using FluentValidation;
using Energy.Shared.Models.V1.HR.TimesheetLine.Requests;

namespace Energy.Application.Modules.HR.TimesheetLine.Validators;

/// <summary>CreateTimesheetLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateTimesheetLineRequestValidator : AbstractValidator<CreateTimesheetLineRequest>
{
    public CreateTimesheetLineRequestValidator()
    {
        RuleFor(x => x.TimesheetId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}
