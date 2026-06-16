using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;

namespace Energy.Application.Operations.WorkOrderChecklist.Validators;

/// <summary>CreateWorkOrderChecklistRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWorkOrderChecklistRequestValidator : AbstractValidator<CreateWorkOrderChecklistRequest>
{
    public CreateWorkOrderChecklistRequestValidator()
    {
        RuleFor(x => x.WorkOrderId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
