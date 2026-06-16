using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialPlan.Validators;

/// <summary>CreateWorkOrderMaterialPlanRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWorkOrderMaterialPlanRequestValidator : AbstractValidator<CreateWorkOrderMaterialPlanRequest>
{
    public CreateWorkOrderMaterialPlanRequestValidator()
    {
        RuleFor(x => x.WorkOrderId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
