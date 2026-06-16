using FluentValidation;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerAddress.Requests;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerAddress.Validators;

/// <summary>UpdateBusinessPartnerAddressRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateBusinessPartnerAddressRequestValidator : AbstractValidator<UpdateBusinessPartnerAddressRequest>
{
    public UpdateBusinessPartnerAddressRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.BusinessPartnerId).NotEmpty();
        RuleFor(x => x.AddressType).NotEmpty();
        RuleFor(x => x.AddressLine).NotEmpty();
    }
}
