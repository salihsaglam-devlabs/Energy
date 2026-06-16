using FluentValidation;
using Energy.Shared.Models.V1.BusinessPartners.BusinessPartnerBankAccount.Requests;

namespace Energy.Application.BusinessPartners.BusinessPartnerBankAccount.Validators;

/// <summary>CreateBusinessPartnerBankAccountRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateBusinessPartnerBankAccountRequestValidator : AbstractValidator<CreateBusinessPartnerBankAccountRequest>
{
    public CreateBusinessPartnerBankAccountRequestValidator()
    {
        RuleFor(x => x.BusinessPartnerId).NotEmpty();
        RuleFor(x => x.BankName).NotEmpty();
        RuleFor(x => x.Iban).NotEmpty();
    }
}
