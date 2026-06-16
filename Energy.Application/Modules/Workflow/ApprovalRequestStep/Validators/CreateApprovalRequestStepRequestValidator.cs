using Energy.Shared.Common;
using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalRequestStep.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalRequestStep.Validators;

/// <summary>CreateApprovalRequestStepRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApprovalRequestStepRequestValidator : AbstractValidator<CreateApprovalRequestStepRequest>
{
    public CreateApprovalRequestStepRequestValidator()
    {
        RuleFor(x => x.ApprovalRequestId).NotEmpty();
        RuleFor(x => x.ApprovalStepDefinitionId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.ApprovalMode).NotEmpty();
    }
}
