using FluentValidation;
using Energy.Shared.Models.V1.Finance.PaymentAllocation.Requests;

namespace Energy.Application.Modules.Finance.PaymentAllocation.Validators;

/// <summary>UpdatePaymentAllocationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdatePaymentAllocationRequestValidator : AbstractValidator<UpdatePaymentAllocationRequest>
{
    public UpdatePaymentAllocationRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.PayableId).NotEmpty();
    }
}
