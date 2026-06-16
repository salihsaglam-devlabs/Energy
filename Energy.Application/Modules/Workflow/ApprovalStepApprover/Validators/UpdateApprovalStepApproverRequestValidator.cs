using Energy.Shared.Common;
using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalStepApprover.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalStepApprover.Validators;

/// <summary>UpdateApprovalStepApproverRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateApprovalStepApproverRequestValidator : AbstractValidator<UpdateApprovalStepApproverRequest>
{
    public UpdateApprovalStepApproverRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ApprovalStepDefinitionId).NotEmpty();
        RuleFor(x => x.ApproverType).NotEmpty();
    }
}
