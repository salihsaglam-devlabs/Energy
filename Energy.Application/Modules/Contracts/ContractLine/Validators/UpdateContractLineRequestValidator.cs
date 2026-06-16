using FluentValidation;
using Energy.Shared.Models.V1.Contracts.ContractLine.Requests;

namespace Energy.Application.Modules.Contracts.ContractLine.Validators;

/// <summary>UpdateContractLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateContractLineRequestValidator : AbstractValidator<UpdateContractLineRequest>
{
    public UpdateContractLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ContractId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
