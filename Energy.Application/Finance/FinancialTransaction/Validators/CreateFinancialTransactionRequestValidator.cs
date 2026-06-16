using FluentValidation;
using Energy.Shared.Models.V1.Finance.FinancialTransaction.Requests;

namespace Energy.Application.Finance.FinancialTransaction.Validators;

/// <summary>CreateFinancialTransactionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateFinancialTransactionRequestValidator : AbstractValidator<CreateFinancialTransactionRequest>
{
    public CreateFinancialTransactionRequestValidator()
    {
        RuleFor(x => x.TransactionType).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}
