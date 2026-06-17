using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Requests;

namespace Energy.Application.Operations.WorkOrderChecklistItem.Validators;

/// <summary>UpdateWorkOrderChecklistItemRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWorkOrderChecklistItemRequestValidator : AbstractValidator<UpdateWorkOrderChecklistItemRequest>
{
    public UpdateWorkOrderChecklistItemRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkOrderChecklistId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
