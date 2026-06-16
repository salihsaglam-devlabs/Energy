using FluentValidation;
using Energy.Shared.Models.V1.Inventory.WarehouseLocation.Requests;

namespace Energy.Application.Inventory.WarehouseLocation.Validators;

/// <summary>CreateWarehouseLocationRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWarehouseLocationRequestValidator : AbstractValidator<CreateWarehouseLocationRequest>
{
    public CreateWarehouseLocationRequestValidator()
    {
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
