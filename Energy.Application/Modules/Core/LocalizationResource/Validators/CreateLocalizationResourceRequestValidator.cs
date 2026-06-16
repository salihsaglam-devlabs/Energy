using FluentValidation;
using Energy.Shared.Models.V1.Core.LocalizationResource.Requests;

namespace Energy.Application.Modules.Core.LocalizationResource.Validators;

/// <summary>CreateLocalizationResourceRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateLocalizationResourceRequestValidator : AbstractValidator<CreateLocalizationResourceRequest>
{
    public CreateLocalizationResourceRequestValidator()
    {
        RuleFor(x => x.Key).NotEmpty();
        RuleFor(x => x.Culture).NotEmpty();
        RuleFor(x => x.Value).NotEmpty();
    }
}
