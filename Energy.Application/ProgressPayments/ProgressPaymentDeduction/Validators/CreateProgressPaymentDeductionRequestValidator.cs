using FluentValidation;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;

namespace Energy.Application.ProgressPayments.ProgressPaymentDeduction.Validators;

/// <summary>CreateProgressPaymentDeductionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProgressPaymentDeductionRequestValidator : AbstractValidator<CreateProgressPaymentDeductionRequest>
{
    public CreateProgressPaymentDeductionRequestValidator()
    {
        RuleFor(x => x.ProgressPaymentId).NotEmpty();
        RuleFor(x => x.DeductionType).NotEmpty();
    }
}
