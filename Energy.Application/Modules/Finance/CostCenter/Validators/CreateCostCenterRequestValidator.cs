using FluentValidation;
using Energy.Shared.Models.V1.Finance.CostCenter.Requests;

namespace Energy.Application.Modules.Finance.CostCenter.Validators;

/// <summary>CreateCostCenterRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateCostCenterRequestValidator : AbstractValidator<CreateCostCenterRequest>
{
    public CreateCostCenterRequestValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
