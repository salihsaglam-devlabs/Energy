using FluentValidation;
using Energy.Shared.Models.V1.Inventory.Warehouse.Requests;

namespace Energy.Application.Modules.Inventory.Warehouse.Validators;

/// <summary>CreateWarehouseRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWarehouseRequestValidator : AbstractValidator<CreateWarehouseRequest>
{
    public CreateWarehouseRequestValidator()
    {
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.WarehouseType).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
