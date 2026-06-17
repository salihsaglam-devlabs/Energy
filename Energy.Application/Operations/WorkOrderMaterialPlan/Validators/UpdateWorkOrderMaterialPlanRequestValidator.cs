using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialPlan.Requests;

namespace Energy.Application.Operations.WorkOrderMaterialPlan.Validators;

/// <summary>UpdateWorkOrderMaterialPlanRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWorkOrderMaterialPlanRequestValidator : AbstractValidator<UpdateWorkOrderMaterialPlanRequest>
{
    public UpdateWorkOrderMaterialPlanRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkOrderId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
