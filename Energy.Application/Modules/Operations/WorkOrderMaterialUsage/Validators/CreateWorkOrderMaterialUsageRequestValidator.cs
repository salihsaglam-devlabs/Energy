using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderMaterialUsage.Requests;

namespace Energy.Application.Modules.Operations.WorkOrderMaterialUsage.Validators;

/// <summary>CreateWorkOrderMaterialUsageRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWorkOrderMaterialUsageRequestValidator : AbstractValidator<CreateWorkOrderMaterialUsageRequest>
{
    public CreateWorkOrderMaterialUsageRequestValidator()
    {
        RuleFor(x => x.WorkOrderId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
