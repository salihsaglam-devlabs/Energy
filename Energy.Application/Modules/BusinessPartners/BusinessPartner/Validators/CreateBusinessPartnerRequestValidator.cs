using FluentValidation;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Requests;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartner.Validators;

/// <summary>CreateBusinessPartnerRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateBusinessPartnerRequestValidator : AbstractValidator<CreateBusinessPartnerRequest>
{
    public CreateBusinessPartnerRequestValidator()
    {
        RuleFor(x => x.PartnerType).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
