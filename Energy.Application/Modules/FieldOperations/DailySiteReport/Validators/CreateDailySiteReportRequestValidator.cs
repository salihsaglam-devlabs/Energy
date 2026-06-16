using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Requests;

namespace Energy.Application.Modules.FieldOperations.DailySiteReport.Validators;

/// <summary>CreateDailySiteReportRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDailySiteReportRequestValidator : AbstractValidator<CreateDailySiteReportRequest>
{
    public CreateDailySiteReportRequestValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.ReportNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
