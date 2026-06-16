using Energy.Shared.Common;
using FluentValidation;
using Energy.Shared.Models.V1.Inventory.Warehouse.Requests;

namespace Energy.Application.Modules.Inventory.Warehouse.Validators;

/// <summary>UpdateWarehouseRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWarehouseRequestValidator : AbstractValidator<UpdateWarehouseRequest>
{
    public UpdateWarehouseRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.CompanyId).NotEmpty();
        RuleFor(x => x.WarehouseType).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}
