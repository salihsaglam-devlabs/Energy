using FluentValidation;
using Energy.Shared.Models.V1.Finance.CostCenter.Requests;

namespace Energy.Application.Finance.CostCenter.Validators;

/// <summary>UpdateCostCenterRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateCostCenterRequestValidator : AbstractValidator<UpdateCostCenterRequest>
{
    public UpdateCostCenterRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
