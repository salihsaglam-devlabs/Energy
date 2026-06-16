using FluentValidation;
using Energy.Shared.Models.V1.Core.Currency.Requests;

namespace Energy.Application.Modules.Core.Currency.Validators;

/// <summary>CreateCurrencyRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateCurrencyRequestValidator : AbstractValidator<CreateCurrencyRequest>
{
    public CreateCurrencyRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
