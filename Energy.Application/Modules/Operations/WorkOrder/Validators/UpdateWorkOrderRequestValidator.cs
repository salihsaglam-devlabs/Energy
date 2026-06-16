using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrder.Requests;

namespace Energy.Application.Modules.Operations.WorkOrder.Validators;

/// <summary>UpdateWorkOrderRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWorkOrderRequestValidator : AbstractValidator<UpdateWorkOrderRequest>
{
    public UpdateWorkOrderRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkOrderTypeId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.WorkOrderNo).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
    }
}
