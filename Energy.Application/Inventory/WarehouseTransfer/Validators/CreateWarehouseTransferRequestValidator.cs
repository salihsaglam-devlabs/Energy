using FluentValidation;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Requests;

namespace Energy.Application.Inventory.WarehouseTransfer.Validators;

/// <summary>CreateWarehouseTransferRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class CreateWarehouseTransferRequestValidator : AbstractValidator<CreateWarehouseTransferRequest>
{
    public CreateWarehouseTransferRequestValidator()
    {
        RuleFor(x => x.SourceWarehouseId).NotEmpty();
        RuleFor(x => x.TargetWarehouseId).NotEmpty();
        RuleFor(x => x.TransferNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
