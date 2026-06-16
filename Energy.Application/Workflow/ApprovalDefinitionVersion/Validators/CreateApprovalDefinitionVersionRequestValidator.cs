using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalDefinitionVersion.Requests;

namespace Energy.Application.Workflow.ApprovalDefinitionVersion.Validators;

/// <summary>CreateApprovalDefinitionVersionRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApprovalDefinitionVersionRequestValidator : AbstractValidator<CreateApprovalDefinitionVersionRequest>
{
    public CreateApprovalDefinitionVersionRequestValidator()
    {
        RuleFor(x => x.ApprovalDefinitionId).NotEmpty();
    }
}
