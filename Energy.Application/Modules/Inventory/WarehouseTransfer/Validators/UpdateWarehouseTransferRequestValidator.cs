using FluentValidation;
using Energy.Shared.Models.V1.Inventory.WarehouseTransfer.Requests;

namespace Energy.Application.Modules.Inventory.WarehouseTransfer.Validators;

/// <summary>UpdateWarehouseTransferRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateWarehouseTransferRequestValidator : AbstractValidator<UpdateWarehouseTransferRequest>
{
    public UpdateWarehouseTransferRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SourceWarehouseId).NotEmpty();
        RuleFor(x => x.TargetWarehouseId).NotEmpty();
        RuleFor(x => x.TransferNo).NotEmpty();
        RuleFor(x => x.Status).NotEmpty();
    }
}
