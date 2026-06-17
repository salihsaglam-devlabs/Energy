using FluentValidation;
using Energy.Shared.Models.V1.Finance.FinancialTransactionLine.Requests;

namespace Energy.Application.Finance.FinancialTransactionLine.Validators;

/// <summary>UpdateFinancialTransactionLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateFinancialTransactionLineRequestValidator : AbstractValidator<UpdateFinancialTransactionLineRequest>
{
    public UpdateFinancialTransactionLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FinancialTransactionId).NotEmpty();
    }
}
