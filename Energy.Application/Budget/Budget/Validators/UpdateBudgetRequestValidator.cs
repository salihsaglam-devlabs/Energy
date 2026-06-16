using FluentValidation;
using Energy.Shared.Models.V1.Budget.Budget.Requests;

namespace Energy.Application.Budget.Budget.Validators;

/// <summary>UpdateBudgetRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateBudgetRequestValidator : AbstractValidator<UpdateBudgetRequest>
{
    public UpdateBudgetRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
