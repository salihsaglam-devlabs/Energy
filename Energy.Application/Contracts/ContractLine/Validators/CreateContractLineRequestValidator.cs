using FluentValidation;
using Energy.Shared.Models.V1.Contracts.ContractLine.Requests;

namespace Energy.Application.Contracts.ContractLine.Validators;

/// <summary>CreateContractLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateContractLineRequestValidator : AbstractValidator<CreateContractLineRequest>
{
    public CreateContractLineRequestValidator()
    {
        RuleFor(x => x.ContractId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
