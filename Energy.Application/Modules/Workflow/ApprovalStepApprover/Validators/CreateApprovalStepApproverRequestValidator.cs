using Energy.Shared.Common;
using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalStepApprover.Validators;

/// <summary>CreateApprovalStepApproverRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApprovalStepApproverRequestValidator : AbstractValidator<CreateApprovalStepApproverRequest>
{
    public CreateApprovalStepApproverRequestValidator()
    {
        RuleFor(x => x.ApprovalStepDefinitionId).NotEmpty();
        RuleFor(x => x.ApproverType).NotEmpty();
    }
}
