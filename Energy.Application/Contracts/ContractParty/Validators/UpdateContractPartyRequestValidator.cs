using FluentValidation;
using Energy.Shared.Models.V1.Contracts.ContractParty.Requests;

namespace Energy.Application.Contracts.ContractParty.Validators;

/// <summary>UpdateContractPartyRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateContractPartyRequestValidator : AbstractValidator<UpdateContractPartyRequest>
{
    public UpdateContractPartyRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ContractId).NotEmpty();
        RuleFor(x => x.BusinessPartnerId).NotEmpty();
        RuleFor(x => x.PartyRole).NotEmpty();
    }
}
