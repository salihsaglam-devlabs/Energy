using FluentValidation;
using Energy.Shared.Models.V1.Finance.Payable.Requests;

namespace Energy.Application.Finance.Payable.Validators;

/// <summary>UpdatePayableRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdatePayableRequestValidator : AbstractValidator<UpdatePayableRequest>
{
    public UpdatePayableRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PartnerId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}
