using FluentValidation;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Requests;

namespace Energy.Application.Finance.FinancialTransaction.Validators;

/// <summary>UpdateFinancialTransactionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateFinancialTransactionRequestValidator : AbstractValidator<UpdateFinancialTransactionRequest>
{
    public UpdateFinancialTransactionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TransactionType).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}
