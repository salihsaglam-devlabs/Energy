using FluentValidation;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;

namespace Energy.Application.Finance.FinancialAccount.Validators;

/// <summary>CreateFinancialAccountRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateFinancialAccountRequestValidator : AbstractValidator<CreateFinancialAccountRequest>
{
    public CreateFinancialAccountRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.AccountType).NotEmpty();
    }
}
