using FluentValidation;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPayment.Requests;

namespace Energy.Application.Modules.ProgressPayments.ProgressPayment.Validators;

/// <summary>CreateProgressPaymentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProgressPaymentRequestValidator : AbstractValidator<CreateProgressPaymentRequest>
{
    public CreateProgressPaymentRequestValidator()
    {
        RuleFor(x => x.ContractId).NotEmpty();
        RuleFor(x => x.ProgressPaymentNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
