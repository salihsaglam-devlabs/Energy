using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;

namespace Energy.Application.Operations.WorkOrderMaterialUsage.Validators;

/// <summary>UpdateWorkOrderMaterialUsageRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWorkOrderMaterialUsageRequestValidator : AbstractValidator<UpdateWorkOrderMaterialUsageRequest>
{
    public UpdateWorkOrderMaterialUsageRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkOrderId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
