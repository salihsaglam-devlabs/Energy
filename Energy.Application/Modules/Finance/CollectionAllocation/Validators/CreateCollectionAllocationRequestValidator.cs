using FluentValidation;
using Energy.Shared.Models.V1.Finance.CollectionAllocation.Requests;

namespace Energy.Application.Modules.Finance.CollectionAllocation.Validators;

/// <summary>CreateCollectionAllocationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateCollectionAllocationRequestValidator : AbstractValidator<CreateCollectionAllocationRequest>
{
    public CreateCollectionAllocationRequestValidator()
    {
        RuleFor(x => x.CollectionId).NotEmpty();
        RuleFor(x => x.ReceivableId).NotEmpty();
    }
}
