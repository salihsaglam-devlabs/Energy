using FluentValidation;
using Energy.Shared.Models.V1.Organization.ExpenseClaimLine.Requests;

namespace Energy.Application.Organization.ExpenseClaimLine.Validators;

/// <summary>CreateExpenseClaimLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateExpenseClaimLineRequestValidator : AbstractValidator<CreateExpenseClaimLineRequest>
{
    public CreateExpenseClaimLineRequestValidator()
    {
        RuleFor(x => x.ExpenseClaimId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
    }
}
