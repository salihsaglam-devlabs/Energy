using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderChecklistItem.Requests;

namespace Energy.Application.Modules.Operations.WorkOrderChecklistItem.Validators;

/// <summary>CreateWorkOrderChecklistItemRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWorkOrderChecklistItemRequestValidator : AbstractValidator<CreateWorkOrderChecklistItemRequest>
{
    public CreateWorkOrderChecklistItemRequestValidator()
    {
        RuleFor(x => x.WorkOrderChecklistId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
