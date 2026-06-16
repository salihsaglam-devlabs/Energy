using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestApprover.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalRequestApprover.Validators;

/// <summary>UpdateApprovalRequestApproverRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateApprovalRequestApproverRequestValidator : AbstractValidator<UpdateApprovalRequestApproverRequest>
{
    public UpdateApprovalRequestApproverRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ApprovalRequestStepId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
