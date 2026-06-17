using FluentValidation;
using Energy.Shared.Models.V1.Operations.WorkOrderAssignment.Requests;

namespace Energy.Application.Operations.WorkOrderAssignment.Validators;

/// <summary>CreateWorkOrderAssignmentRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWorkOrderAssignmentRequestValidator : AbstractValidator<CreateWorkOrderAssignmentRequest>
{
    public CreateWorkOrderAssignmentRequestValidator()
    {
        RuleFor(x => x.WorkOrderId).NotEmpty();
    }
}
