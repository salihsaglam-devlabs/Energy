using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalAction.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalAction.Validators;

/// <summary>CreateApprovalActionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApprovalActionRequestValidator : AbstractValidator<CreateApprovalActionRequest>
{
    public CreateApprovalActionRequestValidator()
    {
        RuleFor(x => x.ApprovalRequestId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ActionType).NotEmpty();
    }
}
