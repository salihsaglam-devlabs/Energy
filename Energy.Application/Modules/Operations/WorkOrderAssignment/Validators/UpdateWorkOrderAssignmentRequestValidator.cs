using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Requests;

namespace Energy.Application.Modules.Operations.WorkOrderAssignment.Validators;

/// <summary>UpdateWorkOrderAssignmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWorkOrderAssignmentRequestValidator : AbstractValidator<UpdateWorkOrderAssignmentRequest>
{
    public UpdateWorkOrderAssignmentRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WorkOrderId).NotEmpty();
    }
}
