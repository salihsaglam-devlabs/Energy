using FluentValidation;
using Energy.Shared.Models.V1.Core.ExchangeRate.Requests;

namespace Energy.Application.Core.ExchangeRate.Validators;

/// <summary>CreateExchangeRateRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateExchangeRateRequestValidator : AbstractValidator<CreateExchangeRateRequest>
{
    public CreateExchangeRateRequestValidator()
    {
        RuleFor(x => x.CurrencyId).NotEmpty();
    }
}
