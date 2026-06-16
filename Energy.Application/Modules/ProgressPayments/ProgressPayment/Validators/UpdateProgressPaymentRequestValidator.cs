using FluentValidation;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Requests;

namespace Energy.Application.Modules.ProgressPayments.ProgressPayment.Validators;

/// <summary>UpdateProgressPaymentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProgressPaymentRequestValidator : AbstractValidator<UpdateProgressPaymentRequest>
{
    public UpdateProgressPaymentRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ContractId).NotEmpty();
        RuleFor(x => x.ProgressPaymentNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
