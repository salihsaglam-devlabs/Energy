using Energy.Shared.Common;
using FluentValidation;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartner.Requests;

namespace Energy.Application.BusinessPartners.BusinessPartner.Validators;

/// <summary>UpdateBusinessPartnerRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateBusinessPartnerRequestValidator : AbstractValidator<UpdateBusinessPartnerRequest>
{
    public UpdateBusinessPartnerRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PartnerType).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
