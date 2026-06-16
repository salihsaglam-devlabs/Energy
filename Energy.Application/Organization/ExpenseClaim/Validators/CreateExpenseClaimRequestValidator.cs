using FluentValidation;
using Energy.Shared.Models.V1.Organization.ExpenseClaim.Requests;

namespace Energy.Application.Organization.ExpenseClaim.Validators;

/// <summary>CreateExpenseClaimRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateExpenseClaimRequestValidator : AbstractValidator<CreateExpenseClaimRequest>
{
    public CreateExpenseClaimRequestValidator()
    {
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.CurrencyId).NotEmpty();
        RuleFor(x => x.ClaimNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
