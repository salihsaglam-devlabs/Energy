using FluentValidation;
using Energy.Shared.Models.V1.Contracts.ContractAmendment.Requests;

namespace Energy.Application.Contracts.ContractAmendment.Validators;

/// <summary>CreateContractAmendmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateContractAmendmentRequestValidator : AbstractValidator<CreateContractAmendmentRequest>
{
    public CreateContractAmendmentRequestValidator()
    {
        RuleFor(x => x.ContractId).NotEmpty();
        RuleFor(x => x.AmendmentNo).NotEmpty();
    }
}
