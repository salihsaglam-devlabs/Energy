using FluentValidation;
using Energy.Shared.Models.V1.Core.Company.Requests;

namespace Energy.Application.Modules.Core.Company.Validators;

/// <summary>UpdateCompanyRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest>
{
    public UpdateCompanyRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.BaseCurrencyId).NotEmpty();
    }
}
