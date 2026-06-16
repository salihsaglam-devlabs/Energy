using FluentValidation;
using Energy.Shared.Models.V1.Core.LocalizationResource.Requests;

namespace Energy.Application.Modules.Core.LocalizationResource.Validators;

/// <summary>UpdateLocalizationResourceRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateLocalizationResourceRequestValidator : AbstractValidator<UpdateLocalizationResourceRequest>
{
    public UpdateLocalizationResourceRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Key).NotEmpty();
        RuleFor(x => x.Culture).NotEmpty();
        RuleFor(x => x.Value).NotEmpty();
    }
}
