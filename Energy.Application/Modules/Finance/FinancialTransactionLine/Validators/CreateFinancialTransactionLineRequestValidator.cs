using FluentValidation;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Requests;

namespace Energy.Application.Modules.Finance.FinancialTransactionLine.Validators;

/// <summary>CreateFinancialTransactionLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateFinancialTransactionLineRequestValidator : AbstractValidator<CreateFinancialTransactionLineRequest>
{
    public CreateFinancialTransactionLineRequestValidator()
    {
        RuleFor(x => x.FinancialTransactionId).NotEmpty();
    }
}
