using FluentValidation;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Requests;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Validators;

/// <summary>CreateBusinessPartnerAddressRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateBusinessPartnerAddressRequestValidator : AbstractValidator<CreateBusinessPartnerAddressRequest>
{
    public CreateBusinessPartnerAddressRequestValidator()
    {
        RuleFor(x => x.BusinessPartnerId).NotEmpty();
        RuleFor(x => x.AddressType).NotEmpty();
        RuleFor(x => x.AddressLine).NotEmpty();
    }
}
