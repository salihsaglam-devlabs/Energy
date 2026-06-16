using Energy.Shared.Common;
using FluentValidation;
using Energy.Shared.Models.V1.Contracts.Contract.Requests;

namespace Energy.Application.Modules.Contracts.Contract.Validators;

/// <summary>UpdateContractRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateContractRequestValidator : AbstractValidator<UpdateContractRequest>
{
    public UpdateContractRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ContractType).NotEmpty();
        RuleFor(x => x.ContractNo).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
