using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklist.Requests;

namespace Energy.Application.Operations.WorkOrderChecklist.Validators;

/// <summary>UpdateWorkOrderChecklistRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWorkOrderChecklistRequestValidator : AbstractValidator<UpdateWorkOrderChecklistRequest>
{
    public UpdateWorkOrderChecklistRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkOrderId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
