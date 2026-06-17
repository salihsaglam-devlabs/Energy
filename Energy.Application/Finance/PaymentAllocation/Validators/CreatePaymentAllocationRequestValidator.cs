using FluentValidation;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Requests;

namespace Energy.Application.Finance.PaymentAllocation.Validators;

/// <summary>CreatePaymentAllocationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreatePaymentAllocationRequestValidator : AbstractValidator<CreatePaymentAllocationRequest>
{
    public CreatePaymentAllocationRequestValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.PayableId).NotEmpty();
    }
}
