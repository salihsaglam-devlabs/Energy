using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalAction.Validators;

/// <summary>UpdateApprovalActionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateApprovalActionRequestValidator : AbstractValidator<UpdateApprovalActionRequest>
{
    public UpdateApprovalActionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ApprovalRequestId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ActionType).NotEmpty();
    }
}
