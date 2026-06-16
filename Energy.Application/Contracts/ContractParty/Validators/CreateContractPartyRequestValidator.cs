using FluentValidation;
using Energy.Shared.Models.V1.Contracts.ContractParty.Requests;

namespace Energy.Application.Contracts.ContractParty.Validators;

/// <summary>CreateContractPartyRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateContractPartyRequestValidator : AbstractValidator<CreateContractPartyRequest>
{
    public CreateContractPartyRequestValidator()
    {
        RuleFor(x => x.ContractId).NotEmpty();
        RuleFor(x => x.BusinessPartnerId).NotEmpty();
        RuleFor(x => x.PartyRole).NotEmpty();
    }
}
