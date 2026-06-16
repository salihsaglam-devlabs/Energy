using FluentValidation;
using Energy.Shared.Models.V1.Budget.BudgetLine.Requests;

namespace Energy.Application.Modules.Budget.BudgetLine.Validators;

/// <summary>CreateBudgetLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateBudgetLineRequestValidator : AbstractValidator<CreateBudgetLineRequest>
{
    public CreateBudgetLineRequestValidator()
    {
        RuleFor(x => x.BudgetId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
