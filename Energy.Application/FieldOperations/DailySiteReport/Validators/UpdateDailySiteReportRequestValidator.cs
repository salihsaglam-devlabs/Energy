using FluentValidation;
using Energy.Shared.Models.V1.FieldOperations.DailySiteReport.Requests;

namespace Energy.Application.FieldOperations.DailySiteReport.Validators;

/// <summary>UpdateDailySiteReportRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDailySiteReportRequestValidator : AbstractValidator<UpdateDailySiteReportRequest>
{
    public UpdateDailySiteReportRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.ReportNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
