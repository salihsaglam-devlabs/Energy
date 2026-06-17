using FluentValidation;
using Energy.Shared.Models.V1.Core.ExchangeRate.Requests;

namespace Energy.Application.Core.ExchangeRate.Validators;

/// <summary>UpdateExchangeRateRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateExchangeRateRequestValidator : AbstractValidator<UpdateExchangeRateRequest>
{
    public UpdateExchangeRateRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}
