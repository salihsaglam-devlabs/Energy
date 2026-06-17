using FluentValidation;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Requests;

namespace Energy.Application.Reporting.DashboardWidget.Validators;

/// <summary>CreateDashboardWidgetRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateDashboardWidgetRequestValidator : AbstractValidator<CreateDashboardWidgetRequest>
{
    public CreateDashboardWidgetRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Module).NotEmpty();
        RuleFor(x => x.WidgetType).NotEmpty();
    }
}
