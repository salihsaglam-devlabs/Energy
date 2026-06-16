using FluentValidation;
using Energy.Shared.Models.V1.Reporting.DashboardWidget.Requests;

namespace Energy.Application.Modules.Reporting.DashboardWidget.Validators;

/// <summary>UpdateDashboardWidgetRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateDashboardWidgetRequestValidator : AbstractValidator<UpdateDashboardWidgetRequest>
{
    public UpdateDashboardWidgetRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Module).NotEmpty();
        RuleFor(x => x.WidgetType).NotEmpty();
    }
}
