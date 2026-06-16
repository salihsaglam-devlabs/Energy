using FluentValidation;
using Energy.Shared.Models.V1.Finance.FinancialAccount.Requests;

namespace Energy.Application.Modules.Finance.FinancialAccount.Validators;

/// <summary>UpdateFinancialAccountRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateFinancialAccountRequestValidator : AbstractValidator<UpdateFinancialAccountRequest>
{
    public UpdateFinancialAccountRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.AccountType).NotEmpty();
    }
}
