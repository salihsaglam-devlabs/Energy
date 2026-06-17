using FluentValidation;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Requests;

namespace Energy.Application.BusinessPartners.BusinessPartnerContact.Validators;

/// <summary>CreateBusinessPartnerContactRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateBusinessPartnerContactRequestValidator : AbstractValidator<CreateBusinessPartnerContactRequest>
{
    public CreateBusinessPartnerContactRequestValidator()
    {
        RuleFor(x => x.BusinessPartnerId).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty();
    }
}
