using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Requests;

namespace Energy.Application.Workflow.ApprovalRequestApprover.Validators;

/// <summary>CreateApprovalRequestApproverRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApprovalRequestApproverRequestValidator : AbstractValidator<CreateApprovalRequestApproverRequest>
{
    public CreateApprovalRequestApproverRequestValidator()
    {
        RuleFor(x => x.ApprovalRequestStepId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
