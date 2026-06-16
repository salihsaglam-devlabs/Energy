using FluentValidation;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Requests;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Validators;

/// <summary>CreateProgressPaymentLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateProgressPaymentLineRequestValidator : AbstractValidator<CreateProgressPaymentLineRequest>
{
    public CreateProgressPaymentLineRequestValidator()
    {
        RuleFor(x => x.ProgressPaymentId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
