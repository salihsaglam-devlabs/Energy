using FluentValidation;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Requests;

namespace Energy.Application.Modules.Contracts.ContractAmendment.Validators;

/// <summary>UpdateContractAmendmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateContractAmendmentRequestValidator : AbstractValidator<UpdateContractAmendmentRequest>
{
    public UpdateContractAmendmentRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ContractId).NotEmpty();
        RuleFor(x => x.AmendmentNo).NotEmpty();
    }
}
