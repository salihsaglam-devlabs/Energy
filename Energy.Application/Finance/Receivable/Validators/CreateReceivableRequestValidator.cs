using FluentValidation;
using Energy.Shared.Models.V1.Finance.Receivable.Requests;

namespace Energy.Application.Finance.Receivable.Validators;

/// <summary>CreateReceivableRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateReceivableRequestValidator : AbstractValidator<CreateReceivableRequest>
{
    public CreateReceivableRequestValidator()
    {
        RuleFor(x => x.PartnerId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}
