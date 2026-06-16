using FluentValidation;
using Energy.Shared.Models.V1.Inventory.StockLot.Requests;

namespace Energy.Application.Modules.Inventory.StockLot.Validators;

/// <summary>UpdateStockLotRequest için doğrulama kuralları (zorunlu alanlar).</summary>
public sealed class UpdateStockLotRequestValidator : AbstractValidator<UpdateStockLotRequest>
{
    public UpdateStockLotRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.MaterialId).NotEmpty();
        RuleFor(x => x.SourceStockDocumentLineId).NotEmpty();
        RuleFor(x => x.LotNo).NotEmpty();
    }
}
