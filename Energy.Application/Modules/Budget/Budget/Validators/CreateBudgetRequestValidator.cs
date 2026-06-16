using FluentValidation;
using Energy.Shared.Models.V1.Budget.Budget.Requests;

namespace Energy.Application.Modules.Budget.Budget.Validators;

/// <summary>CreateBudgetRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateBudgetRequestValidator : AbstractValidator<CreateBudgetRequest>
{
    public CreateBudgetRequestValidator()
    {
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
