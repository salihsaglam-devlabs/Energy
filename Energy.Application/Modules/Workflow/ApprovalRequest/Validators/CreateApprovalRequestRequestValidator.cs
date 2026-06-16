using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalRequest.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalRequest.Validators;

/// <summary>CreateApprovalRequestRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApprovalRequestRequestValidator : AbstractValidator<CreateApprovalRequestRequest>
{
    public CreateApprovalRequestRequestValidator()
    {
        RuleFor(x => x.ApprovalDefinitionVersionId).NotEmpty();
        RuleFor(x => x.RelatedModule).NotEmpty();
        RuleFor(x => x.RelatedEntityType).NotEmpty();
        RuleFor(x => x.RelatedEntityId).NotEmpty();
        RuleFor(x => x.RequestedByUserId).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
