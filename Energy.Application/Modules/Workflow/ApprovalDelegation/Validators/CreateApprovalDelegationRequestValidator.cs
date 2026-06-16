using FluentValidation;
using Energy.Shared.Models.V1.Workflow.ApprovalDelegation.Requests;

namespace Energy.Application.Modules.Workflow.ApprovalDelegation.Validators;

/// <summary>CreateApprovalDelegationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateApprovalDelegationRequestValidator : AbstractValidator<CreateApprovalDelegationRequest>
{
    public CreateApprovalDelegationRequestValidator()
    {
        RuleFor(x => x.DelegatorUserId).NotEmpty();
        RuleFor(x => x.DelegateUserId).NotEmpty();
    }
}
