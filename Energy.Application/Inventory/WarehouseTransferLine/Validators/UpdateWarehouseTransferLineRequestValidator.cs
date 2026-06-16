using FluentValidation;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Requests;

namespace Energy.Application.Inventory.WarehouseTransferLine.Validators;

/// <summary>UpdateWarehouseTransferLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWarehouseTransferLineRequestValidator : AbstractValidator<UpdateWarehouseTransferLineRequest>
{
    public UpdateWarehouseTransferLineRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseTransferId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
