using FluentValidation;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Requests;

namespace Energy.Application.Inventory.WarehouseLocation.Validators;

/// <summary>UpdateWarehouseLocationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWarehouseLocationRequestValidator : AbstractValidator<UpdateWarehouseLocationRequest>
{
    public UpdateWarehouseLocationRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
