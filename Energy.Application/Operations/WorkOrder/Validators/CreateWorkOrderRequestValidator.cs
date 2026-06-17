using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrder.Requests;

namespace Energy.Application.Operations.WorkOrder.Validators;

/// <summary>CreateWorkOrderRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWorkOrderRequestValidator : AbstractValidator<CreateWorkOrderRequest>
{
    public CreateWorkOrderRequestValidator()
    {
        RuleFor(x => x.WorkOrderTypeId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.WorkOrderNo).NotEmpty();
        RuleFor(x => x.Title).NotEmpty();
    }
}
