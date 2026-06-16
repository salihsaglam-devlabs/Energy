using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalCondition.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalCondition.Validators;

/// <summary>UpdateApprovalConditionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateApprovalConditionRequestValidator : AbstractValidator<UpdateApprovalConditionRequest>
{
    public UpdateApprovalConditionRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ApprovalDefinitionVersionId).NotEmpty();
        RuleFor(x => x.FieldName).NotEmpty();
        RuleFor(x => x.Operator).NotEmpty();
    }
}
