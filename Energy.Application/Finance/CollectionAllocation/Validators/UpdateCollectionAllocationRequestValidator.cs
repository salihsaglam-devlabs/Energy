using FluentValidation;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Requests;

namespace Energy.Application.Finance.CollectionAllocation.Validators;

/// <summary>UpdateCollectionAllocationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateCollectionAllocationRequestValidator : AbstractValidator<UpdateCollectionAllocationRequest>
{
    public UpdateCollectionAllocationRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CollectionId).NotEmpty();
        RuleFor(x => x.ReceivableId).NotEmpty();
    }
}
