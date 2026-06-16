using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReportWorker.Requests;

namespace Energy.Application.Modules.FieldOperations.DailySiteReportWorker.Validators;

/// <summary>CreateDailySiteReportWorkerRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDailySiteReportWorkerRequestValidator : AbstractValidator<CreateDailySiteReportWorkerRequest>
{
    public CreateDailySiteReportWorkerRequestValidator()
    {
        RuleFor(x => x.DailySiteReportId).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
    }
}
