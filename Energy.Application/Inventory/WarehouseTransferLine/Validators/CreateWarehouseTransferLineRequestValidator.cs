using FluentValidation;
using Energy.Shared.Models.V1.Inventory.WarehouseTransferLine.Requests;

namespace Energy.Application.Inventory.WarehouseTransferLine.Validators;

/// <summary>CreateWarehouseTransferLineRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWarehouseTransferLineRequestValidator : AbstractValidator<CreateWarehouseTransferLineRequest>
{
    public CreateWarehouseTransferLineRequestValidator()
    {
        RuleFor(x => x.WarehouseTransferId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
    }
}
