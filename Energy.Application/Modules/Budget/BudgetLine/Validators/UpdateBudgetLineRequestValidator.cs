using FluentValidation;
using Energy.Shared.Models.V1.Budget.BudgetLine.Requests;

namespace Energy.Application.Modules.Budget.BudgetLine.Validators;

/// <summary>UpdateBudgetLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateBudgetLineRequestValidator : AbstractValidator<UpdateBudgetLineRequest>
{
    public UpdateBudgetLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.BudgetId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
