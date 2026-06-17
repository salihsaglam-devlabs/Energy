using FluentValidation;
using Energy.Shared.Models.V1.Reporting.ReportDefinition.Requests;

namespace Energy.Application.Reporting.ReportDefinition.Validators;

/// <summary>UpdateReportDefinitionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateReportDefinitionRequestValidator : AbstractValidator<UpdateReportDefinitionRequest>
{
    public UpdateReportDefinitionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Module).NotEmpty();
        RuleFor(x => x.QueryKey).NotEmpty();
    }
}
