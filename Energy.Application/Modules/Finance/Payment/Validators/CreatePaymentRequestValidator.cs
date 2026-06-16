using FluentValidation;
using Energy.Shared.Models.V1.Finance.Payment.Requests;

namespace Energy.Application.Modules.Finance.Payment.Validators;

/// <summary>CreatePaymentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
{
    public CreatePaymentRequestValidator()
    {
        RuleFor(x => x.PartnerId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.PaymentNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
