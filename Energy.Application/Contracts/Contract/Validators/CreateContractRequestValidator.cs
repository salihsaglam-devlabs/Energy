using Energy.Shared.Common;
using FluentValidation;
using Energy.Shared.Models.V1.Contracts.Contract.Requests;

namespace Energy.Application.Contracts.Contract.Validators;

/// <summary>CreateContractRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateContractRequestValidator : AbstractValidator<CreateContractRequest>
{
    public CreateContractRequestValidator()
    {
        RuleFor(x => x.ContractType).NotEmpty();
        RuleFor(x => x.ContractNo).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
