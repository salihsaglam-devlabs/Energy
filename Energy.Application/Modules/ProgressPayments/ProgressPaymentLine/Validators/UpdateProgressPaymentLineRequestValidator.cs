using FluentValidation;
using Energy.Shared.Models.V1.ProgressPayments.ProgressPaymentLine.Requests;

namespace Energy.Application.Modules.ProgressPayments.ProgressPaymentLine.Validators;

/// <summary>UpdateProgressPaymentLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateProgressPaymentLineRequestValidator : AbstractValidator<UpdateProgressPaymentLineRequest>
{
    public UpdateProgressPaymentLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProgressPaymentId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
