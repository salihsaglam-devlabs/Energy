using FluentValidation;
using Energy.Shared.Models.V1.Core.Company.Requests;

namespace Energy.Application.Modules.Core.Company.Validators;

/// <summary>CreateCompanyRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.BaseCurrencyId).NotEmpty();
    }
}
