using FluentValidation;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerContact.Requests;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerContact.Validators;

/// <summary>UpdateBusinessPartnerContactRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateBusinessPartnerContactRequestValidator : AbstractValidator<UpdateBusinessPartnerContactRequest>
{
    public UpdateBusinessPartnerContactRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.BusinessPartnerId).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty();
    }
}
