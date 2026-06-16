using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalRequestStep.Validators;

/// <summary>UpdateApprovalRequestStepRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateApprovalRequestStepRequestValidator : AbstractValidator<UpdateApprovalRequestStepRequest>
{
    public UpdateApprovalRequestStepRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ApprovalRequestId).NotEmpty();
        RuleFor(x => x.ApprovalStepDefinitionId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.ApprovalMode).NotEmpty();
    }
}
