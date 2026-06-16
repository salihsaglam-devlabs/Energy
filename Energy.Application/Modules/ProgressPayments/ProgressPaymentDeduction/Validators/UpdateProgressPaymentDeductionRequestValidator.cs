using FluentValidation;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentDeduction.Requests;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentDeduction.Validators;

/// <summary>UpdateProgressPaymentDeductionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProgressPaymentDeductionRequestValidator : AbstractValidator<UpdateProgressPaymentDeductionRequest>
{
    public UpdateProgressPaymentDeductionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProgressPaymentId).NotEmpty();
        RuleFor(x => x.DeductionType).NotEmpty();
    }
}
