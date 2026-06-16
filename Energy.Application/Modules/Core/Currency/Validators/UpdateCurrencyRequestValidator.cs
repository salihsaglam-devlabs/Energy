using FluentValidation;
using Energy.Shared.Models.V1.Core.Currency.Requests;

namespace Energy.Application.Modules.Core.Currency.Validators;

/// <summary>UpdateCurrencyRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateCurrencyRequestValidator : AbstractValidator<UpdateCurrencyRequest>
{
    public UpdateCurrencyRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
