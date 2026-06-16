using FluentValidation;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;

namespace Energy.Application.Modules.BusinessPartners.BusinessPartnerBankAccount.Validators;

/// <summary>UpdateBusinessPartnerBankAccountRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateBusinessPartnerBankAccountRequestValidator : AbstractValidator<UpdateBusinessPartnerBankAccountRequest>
{
    public UpdateBusinessPartnerBankAccountRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.BusinessPartnerId).NotEmpty();
        RuleFor(x => x.BankName).NotEmpty();
        RuleFor(x => x.Iban).NotEmpty();
    }
}
