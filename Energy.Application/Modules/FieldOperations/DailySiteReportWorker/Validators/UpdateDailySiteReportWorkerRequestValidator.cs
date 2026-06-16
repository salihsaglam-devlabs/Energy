using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Requests;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Validators;

/// <summary>UpdateDailySiteReportWorkerRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDailySiteReportWorkerRequestValidator : AbstractValidator<UpdateDailySiteReportWorkerRequest>
{
    public UpdateDailySiteReportWorkerRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DailySiteReportId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}
